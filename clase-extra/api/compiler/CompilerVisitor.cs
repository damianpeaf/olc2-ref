
using analyzer;
using Antlr4.Runtime.Misc;

public class FunctionMetadata
{
    public int FrameSize;
    public StackObject.StackObjectType ReturnType;

}

public class CompilerVisitor : LanguageBaseVisitor<Object?>
{

    public ArmGenerator c = new ArmGenerator();
    private string continueLabel = "";
    private string breakLabel = "";
    private string returnLabel = "";
    private Dictionary<string, FunctionMetadata> functions = new Dictionary<string, FunctionMetadata>();
    private string? insideFunction = null;
    private int framePointerOffset = 0;


    public CompilerVisitor()
    {

    }
    public override Object? VisitProgram(LanguageParser.ProgramContext context)
    {
        foreach (var dcl in context.dcl())
        {
            Visit(dcl);
        }
        return null;
    }
    public override Object? VisitVarDcl(LanguageParser.VarDclContext context)
    {
        var varName = context.ID().GetText();
        c.Comment("Variable declaration: " + varName);
        Visit(context.expr());


        if (insideFunction != null)
        {
            var localObject = c.GetFrameLocal(framePointerOffset);
            var valueObject = c.PopObject(Register.X0); // Pop the value to assign

            c.Mov(Register.X1, framePointerOffset * 8); // Move the offset to X1
            c.Sub(Register.X1, Register.FP, Register.X1); // Add the offset to FP to get the address
            c.Str(Register.X0, Register.X1); // Store the value at the address

            localObject.Type = valueObject.Type;
            framePointerOffset++;

            return null;
        }


        c.TagObject(varName);


        return null;
    }
    public override Object? VisitExprStmt(LanguageParser.ExprStmtContext context)
    {
        c.Comment("Expression statement");
        Visit(context.expr());
        c.Comment("Popping value to discard");
        c.PopObject(Register.X0); // Pop the value to discard it

        return null;
    }
    public override Object? VisitPrintStmt(LanguageParser.PrintStmtContext context)
    {
        c.Comment("Print statement");
        c.Comment("Visiting expression");
        Visit(context.expr());

        c.Comment("Popping value to print");
        var isDouble = c.TopObject().Type == StackObject.StackObjectType.Float;
        var value = c.PopObject(isDouble ? Register.D0 : Register.X0); // Pop the value to print

        if (value.Type == StackObject.StackObjectType.Int)
        {
            c.PrintInteger(Register.X0); // Call the print function
        }
        else if (value.Type == StackObject.StackObjectType.String)
        {
            c.PrintString(Register.X0); // Call the print function
        }
        else if (value.Type == StackObject.StackObjectType.Float)
        {
            c.PrintFloat(); // Call the print function
        }

        return null;
    }
    public override Object? VisitIdentifier(LanguageParser.IdentifierContext context)
    {

        var id = context.ID().GetText();

        var (offset, obj) = c.GetObject(id); // Get the variable object

        if (insideFunction != null)
        {
            c.Mov(Register.X0, obj.Offset * 8); // Move the offset to X0
            c.Sub(Register.X0, Register.FP, Register.X0); // Add the offset to FP to get the address
            c.Ldr(Register.X0, Register.X0); // Load the value from the address
            c.Push(Register.X0); // Push the value to the stack
            return null;
        }

        c.Mov(Register.X0, offset); // Move the offset to X0
        c.Add(Register.X0, Register.SP, Register.X0); // Add the offset to SP to get the address

        c.Ldr(Register.X0, Register.X0); // Load the value from the address
        c.Push(Register.X0); // Push the value to the stack

        var newObject = c.CloneObject(obj); // Clone the object
        newObject.Id = null;
        c.PushObject(newObject); // Push the object to the stack

        return null;
    }


    public override Object? VisitAssign(LanguageParser.AssignContext context)
    {

        var assignee = context.expr(0);


        if (assignee is LanguageParser.IdentifierContext idContext)
        {
            string varName = idContext.ID().GetText();

            c.Comment("Assignment to variable: " + varName);

            Visit(context.expr(1));

            var valueObject = c.PopObject(Register.X0); // Pop the value to assign

            var (offset, varObject) = c.GetObject(varName); // Get the variable object

            if (insideFunction != null)
            {
                c.Mov(Register.X1, varObject.Offset * 8); // Move the offset to X1
                c.Sub(Register.X1, Register.FP, Register.X1); // Add the offset to FP to get the address
                c.Str(Register.X0, Register.X1); // Store the value at the address
                return null;
            }

            c.Mov(Register.X1, offset); // Move the offset to X1
            c.Add(Register.X1, Register.SP, Register.X1); // Add the offset to SP to get the address
            c.Str(Register.X0, Register.X1); // Store the value at the address

            varObject.Type = valueObject.Type;

            c.Push(Register.X0); // Push the value back to the stack
            c.PushObject(c.CloneObject(varObject)); // Push the variable object back to the stack

        }


        return null;

    }


    public override Object? VisitInt(LanguageParser.IntContext context)
    {
        var value = context.INT().GetText();
        c.Comment("Constant: " + value);

        var intObject = c.IntObject();
        c.PushConstant(intObject, int.Parse(value)); // Push the integer constant to the stack

        return null;
    }
    public override Object? VisitAddSub(LanguageParser.AddSubContext context)
    {
        c.Comment("Add/Subtract operation");
        var operation = context.op.Text;

        // 1 + (2+3)
        // TOP -> []
        c.Comment("Visiting left operand");
        Visit(context.expr(0));  // Visit 1; TOP -> [1]
        c.Comment("Visiting right operand");
        Visit(context.expr(1));  // Visit 2; TOP -> [2, 1]

        c.Comment("Popping operands");
        var isRightDouble = c.TopObject().Type == StackObject.StackObjectType.Float;
        var right = c.PopObject(isRightDouble ? Register.D0 : Register.X0); // Pop 2; TOP -> [1]
        var isLeftDouble = c.TopObject().Type == StackObject.StackObjectType.Float;
        var left = c.PopObject(isLeftDouble ? Register.D1 : Register.X1); // Pop 1; TOP -> []

        if (isLeftDouble || isRightDouble)
        {
            c.Comment("Converting to double");

            if (!isLeftDouble) c.Scvtf(Register.D1, Register.X1); // Convert left operand to double
            if (!isRightDouble) c.Scvtf(Register.D0, Register.X0); // Convert right operand to double

            if (operation == "+")
            {
                c.Fadd(Register.D0, Register.D0, Register.D1); // D0 = D0 + D1
            }
            else if (operation == "-")
            {
                c.Fsub(Register.D0, Register.D0, Register.D1); // D0 = D0 - D1
            }

            c.Push(Register.D0); // Push result; TOP -> [result]
            c.PushObject(c.CloneObject(isLeftDouble ? left : right)); // Push the object of the left operand
            return null;
        }

        if (operation == "+")
        {
            c.Add(Register.X0, Register.X0, Register.X1); // X0 = X0 + X1
        }
        else if (operation == "-")
        {
            c.Sub(Register.X0, Register.X0, Register.X1); // X0 = X0 - X1
        }


        c.Comment("Pushing result");
        c.Push(Register.X0); // Push result; TOP -> [result]
        c.PushObject(c.CloneObject(left));

        return null;
    }

    public override Object? VisitRelational(LanguageParser.RelationalContext context)
    {
        c.Comment("Relational operation");
        var operation = context.op.Text;
        c.Comment("Visiting left operand");
        Visit(context.expr(0));  // Visit left operand; TOP -> [left]
        c.Comment("Visiting right operand");
        Visit(context.expr(1));  // Visit right operand; TOP -> [right, left]
        c.Comment("Popping operands");

        var isRightDouble = c.TopObject().Type == StackObject.StackObjectType.Float;
        var right = c.PopObject(isRightDouble ? Register.D0 : Register.X0); // Pop right operand; TOP -> [left]
        var isLeftDouble = c.TopObject().Type == StackObject.StackObjectType.Float;
        var left = c.PopObject(isLeftDouble ? Register.D1 : Register.X1); // Pop left operand; TOP -> []

        if (isLeftDouble || isRightDouble)
        {
            // TODO
            return null;
        }

        // Handle integer comparison

        c.Cmp(Register.X1, Register.X0); // Compare left and right operands
        c.Comment("Setting condition flags");

        var trueLabel = c.GetLabel();
        var endLabel = c.GetLabel();

        /*
            push 0
            b end
            trueLabel:
                push 1
            end:
        */

        switch (operation)
        {
            case "<":
                c.Blt(trueLabel); // If left < right, branch to trueLabel
                break;
            case "<=":
                c.Ble(trueLabel); // If left <= right, branch to trueLabel
                break;
            case ">":
                c.Bgt(trueLabel); // If left > right, branch to trueLabel
                break;
            case ">=":
                c.Bge(trueLabel); // If left >= right, branch to trueLabel
                break;
        }

        c.Push(Register.XZR); // Push result to stack
        c.B(endLabel); // Branch to endLabel
        c.SetLabel(trueLabel); // Set trueLabel
        c.Mov(Register.X0, 1); // Set result to true (1)
        c.Push(Register.X0); // Push result to stack
        c.SetLabel(endLabel); // Set endLabel

        c.PushObject(c.BoolObject()); // Push boolean object to stack

        return null;
    }

    public override Object? VisitString(LanguageParser.StringContext context)
    {
        var value = context.STRING().GetText().Trim('"');
        c.Comment("String constant: " + value);
        var stringObject = c.StringObject();
        c.PushConstant(stringObject, value); // Push the string constant to the stack

        return null;
    }
    public override Object? VisitBlockStmt(LanguageParser.BlockStmtContext context)
    {
        c.Comment("Block statement");
        c.NewScope();

        foreach (var dcl in context.dcl())
        {
            Visit(dcl);
        }

        int bytesToRemove = c.endScope();

        if (bytesToRemove > 0)
        {
            c.Comment("Removing " + bytesToRemove + " bytes from stack");
            c.Mov(Register.X0, bytesToRemove);
            c.Add(Register.SP, Register.SP, Register.X0); // Adjust stack pointer
            c.Comment("Stack pointer adjusted");
        }


        return null;
    }


    // VisitFloat
    public override Object? VisitFloat(LanguageParser.FloatContext context)
    {
        var value = context.FLOAT().GetText();
        c.Comment("Constant: " + value);

        var floatObject = c.FloatObject();
        c.PushConstant(floatObject, double.Parse(value)); // Push the float constant to the stack

        return null;
    }

    // VisitBoolean
    public override Object? VisitBoolean(LanguageParser.BooleanContext context)
    {
        var value = bool.Parse(context.BOOL().GetText());

        c.Comment("Constant: " + value);
        var boolObject = c.BoolObject();
        c.PushConstant(boolObject, value);

        return null;
    }

    public override Object? VisitIfStmt(LanguageParser.IfStmtContext context)
    {
        c.Comment("If statement");
        Visit(context.expr());
        c.PopObject(Register.X0); // Pop the condition value

        /*
        
        HAY 2 casos

        SOLO UN IF

        if (!cond) goto end
            ...
        end:
            ...

        IF + ELSE
        if (!cond) goto else
            ...
        goto end
        else:
            ...
        end:

        */

        var hasElse = context.stmt(1);

        if (hasElse != null)
        {
            var elseLabel = c.GetLabel();
            var endLabel = c.GetLabel();

            c.Cbz(Register.X0, elseLabel); // If condition is false, jump to else
            c.Comment("Visiting if block");
            Visit(context.stmt(0)); // Visit the if block
            c.B(endLabel); // Jump to end
            c.SetLabel(elseLabel); // Set the else label
            c.Comment("Visiting else block");
            Visit(hasElse); // Visit the else block
            c.SetLabel(endLabel); // Set the end label
        }
        else
        {
            var endLabel = c.GetLabel();
            c.Cbz(Register.X0, endLabel); // If condition is false, jump to end
            c.Comment("Visiting if block");
            Visit(context.stmt(0)); // Visit the if block
            c.SetLabel(endLabel); // Set the end label
        }


        return null;
    }

    public override Object? VisitWhileStmt(LanguageParser.WhileStmtContext context)
    {

        /*
            start:
                condition
                if (!cond) goto end
                ...
                goto start
            end:
        */

        var previousContinueLabel = continueLabel;
        var previousBreakLabel = breakLabel;

        var startLabel = c.GetLabel();
        var endLabel = c.GetLabel();

        continueLabel = startLabel;
        breakLabel = endLabel;

        c.SetLabel(startLabel); // Set the start label
        c.Comment("Visiting while condition");
        Visit(context.expr()); // Visit the condition
        c.PopObject(Register.X0); // Pop the condition value
        c.Cbz(Register.X0, endLabel); // If condition is false, jump to end
        c.Comment("Visiting while block");
        Visit(context.stmt()); // Visit the while block
        c.B(startLabel); // Jump to start
        c.SetLabel(endLabel); // Set the end label

        continueLabel = previousContinueLabel;
        breakLabel = previousBreakLabel;

        return null;
    }

    public override Object? VisitForStmt(LanguageParser.ForStmtContext context)
    {
        /*
            {
                ...init
                start:
                    condition
                    if (!cond) goto end
                    ...
                    increment:
                        ...increment
                    goto start
                end:
            }
        */

        var previousContinueLabel = continueLabel;
        var previousBreakLabel = breakLabel;

        var startLabel = c.GetLabel();
        var endLabel = c.GetLabel();
        var incrementLabel = c.GetLabel();

        continueLabel = incrementLabel;
        breakLabel = endLabel;

        c.NewScope();

        c.Comment("Visiting for initialization");
        Visit(context.forInit()); // Visit the initialization expression
        c.Comment("Visiting for condition");
        c.SetLabel(startLabel); // Set the start label
        Visit(context.expr(0)); // Visit the condition expression
        c.commentStack(); // Print the stack for debugging
        c.PopObject(Register.X0); // Pop the condition value
        c.Cbz(Register.X0, endLabel); // If condition is false, jump to end
        c.Comment("Visiting for block");
        Visit(context.stmt()); // Visit the for block
        c.SetLabel(incrementLabel); // Set the increment label
        c.Comment("Visiting for increment");
        Visit(context.expr(1)); // Visit the increment expression
        c.B(startLabel); // Jump to start
        c.SetLabel(endLabel); // Set the end label

        var bytesToRemove = c.endScope();
        if (bytesToRemove > 0)
        {
            c.Comment("Removing " + bytesToRemove + " bytes from stack");
            c.Mov(Register.X0, bytesToRemove);
            c.Add(Register.SP, Register.SP, Register.X0); // Adjust stack pointer
            c.Comment("Stack pointer adjusted");
        }

        c.Comment("End of for statement");

        continueLabel = previousContinueLabel;
        breakLabel = previousBreakLabel;

        return null;
    }


    public override Object? VisitBreakStmt(LanguageParser.BreakStmtContext context)
    {
        c.Comment("Break statement");
        c.B(breakLabel); // Jump to break label
        return null;
    }

    public override Object? VisitContinueStmt(LanguageParser.ContinueStmtContext context)
    {
        c.Comment("Continue statement");
        c.B(continueLabel); // Jump to continue label
        return null;
    }

    StackObject.StackObjectType GetType(string name)
    {
        switch (name)
        {
            case "int":
                return StackObject.StackObjectType.Int;
            case "float":
                return StackObject.StackObjectType.Float;
            case "string":
                return StackObject.StackObjectType.String;
            case "bool":
                return StackObject.StackObjectType.Bool;
            default:
                throw new ArgumentException("Invalid function type");
        }
    }


    // VisitFuncDcl
    public override Object? VisitFuncDcl(LanguageParser.FuncDclContext context)
    {
        /*
            Frame will look like this:
            | RA | FP | ...params | ...locals | return address |
        */

        int baseOffset = 2;
        int paramsOffset = 0;

        if (context.@params() != null)
        {
            paramsOffset = context.@params().param().Length;
        }

        FrameVisitor frameVisitor = new FrameVisitor(baseOffset + paramsOffset);


        foreach (var dcl in context.dcl())
        {
            frameVisitor.Visit(dcl);
        }

        var frame = frameVisitor.Frame;
        int localOffset = frameVisitor.LocalOffset;
        int returnOffset = frameVisitor.BaseOffset;

        int totalFrameSize = baseOffset + paramsOffset + localOffset + returnOffset;

        string funcName = context.ID(0).GetText();
        StackObject.StackObjectType funcType = GetType(context.ID(1).GetText());


        functions.Add(funcName, new FunctionMetadata
        {
            FrameSize = totalFrameSize,
            ReturnType = funcType
        });


        var prevInstrucions = c.instructions;
        c.instructions = new List<string>();

        var paramCounter = 0;
        foreach (var param in context.@params().param())
        {
            c.PushObject(new StackObject
            {
                Type = GetType(param.ID(1).GetText()),
                Id = param.ID(0).GetText(),
                Offset = paramCounter + baseOffset,
                Length = 8
            });
            paramCounter++;
        }

        foreach (FrameElement element in frame)
        {
            c.PushObject(new StackObject
            {
                Type = StackObject.StackObjectType.Undefined,
                Id = element.Name,
                Offset = element.Offset,
                Length = 8
            });
        }

        insideFunction = funcName;
        framePointerOffset = 0;

        returnLabel = c.GetLabel();

        c.Comment("Function declaration: " + funcName);
        c.SetLabel(funcName); // Set the function label

        foreach (var dcl in context.dcl())
        {
            Visit(dcl);
        }

        c.SetLabel(returnLabel); // Set the return label

        c.Add(Register.X0, Register.FP, Register.XZR); // Get in X0 the return address
        c.Ldr(Register.LR, Register.X0); // Load the return address from the frame pointer
        c.Br(Register.LR); // Branch to the return address

        c.Comment("End of function: " + funcName);

        // Clean up the stack
        for (int i = 0; i < paramsOffset + localOffset; i++)
        {
            c.PopObject(); //!!!! <-- NEW METHOD
        }

        foreach (var instrucion in c.instructions)
        {
            c.funcInstrucions.Add(instrucion);
        }
        c.instructions = prevInstrucions;

        insideFunction = null;

        return null;
    }

    public override Object? VisitCallee(LanguageParser.CalleeContext context)
    {
        if (context.expr() is not LanguageParser.IdentifierContext idContext) return null;
        if (context.call() is not LanguageParser.FuncCallContext[] callsContext) return null;

        string funcName = idContext.ID().GetText();
        var callContext = callsContext[0];

        // TODO: Embeded functions call

        var postFuncCallLabel = c.GetLabel();

        // 1.  | RA | FP |
        int baseOffset = 2;
        int stackElementSize = 8;
        c.Mov(Register.X0, baseOffset * stackElementSize); // Move the base offset to X0
        c.Sub(Register.SP, Register.SP, Register.X0); // Subtract the base offset from SP

        // 2.  | RA | FP | ...params |
        if (callContext.args() != null)
        {
            c.Comment("Visiting function parameters");
            foreach (var param in callContext.args().expr())
            {
                Visit(param); // Visit the parameters
            }
        }

        // 3. Calcular el valor del FP
        // Regresar el SP al inicio del frame
        c.Mov(Register.X0, stackElementSize * (baseOffset + callContext.args().expr().Length)); // Move the offset to X0
        c.Add(Register.SP, Register.SP, Register.X0); // Add the offset to SP

        // Calcular la posición donde se almacena el FP
        c.Mov(Register.X0, stackElementSize);
        c.Sub(Register.X0, Register.SP, Register.X0); // En x0 se debería de almacenar el FP

        c.Adr(Register.X1, postFuncCallLabel); // Get the address of the function
        c.Push(Register.X1); // Push the address to the stack

        c.Push(Register.FP); // Guardar el FP anterior
        c.Add(Register.FP, Register.X0, Register.XZR); // Actualizar el FP

        // Alinear el sp al final del frame
        int frameSize = functions[funcName].FrameSize;
        c.Mov(Register.X0, (frameSize - 2) * stackElementSize); // Move the frame size to X0
        c.Sub(Register.SP, Register.SP, Register.X0); // Subtract the frame size from SP

        c.Comment("Calling function: " + funcName);
        c.Bl(funcName); // Branch to the function
        c.Comment("Function call complete");
        c.SetLabel(postFuncCallLabel); // Set the start call label

        // obtener el valor de retorno
        var returnOffset = frameSize - 1;
        c.Mov(Register.X4, returnOffset * stackElementSize); // Move the return offset to X4
        c.Sub(Register.X4, Register.FP, Register.X4); // Add the return offset to FP
        c.Ldr(Register.X4, Register.X4); // Load the return value from the frame pointer

        // 4. Regresar el FP al contexto de ejecución anterior
        c.Mov(Register.X1, stackElementSize);
        c.Sub(Register.X1, Register.FP, Register.X1); // Add the stack element size to SP
        c.Ldr(Register.FP, Register.X1); // Load the previous frame pointer

        // 5. Regresar el SP al contexto de ejecución anterior
        c.Mov(Register.X0, stackElementSize * frameSize); // Move the frame size to X0
        c.Add(Register.SP, Register.SP, Register.X0); // Add the frame size to SP

        // 6. Regresar el valor de retorno
        c.Push(Register.X4); // Push the return value to the stack
        c.PushObject(new StackObject
        {
            Type = functions[funcName].ReturnType,
            Id = null,
            Offset = 0,
            Length = 8
        });

        c.Comment("End of function call: " + funcName);

        return null;
    }



}