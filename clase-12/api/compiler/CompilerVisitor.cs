
using analyzer;

public class CompilerVisitor : LanguageBaseVisitor<Object?>
{

    public ArmGenerator c = new ArmGenerator();

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
        var value = c.PopObject(Register.X0); // Pop the value to print

        if (value.Type == StackObject.StackObjectType.Int)
        {
            c.PrintInteger(Register.X0); // Call the print function
        }
        else if (value.Type == StackObject.StackObjectType.String)
        {
            c.PrintString(Register.X0); // Call the print function
        }

        return null;
    }
    public override Object? VisitIdentifier(LanguageParser.IdentifierContext context)
    {

        var id = context.ID().GetText();

        var (offset, obj) = c.GetObject(id); // Get the variable object

        c.Mov(Register.X0, offset); // Move the offset to X0
        c.Add(Register.X0, Register.SP, Register.X0); // Add the offset to SP to get the address

        c.Ldr(Register.X0, Register.X0); // Load the value from the address
        c.Push(Register.X0); // Push the value to the stack

        var newObject = c.CloneObject(obj); // Clone the object
        newObject.Id = null;
        c.PushObject(newObject); // Push the object to the stack

        return null;
    }


    public override ValueWrapper VisitAssign(LanguageParser.AssignContext context)
    {

        var assignee = context.expr(0);


        if (assignee is LanguageParser.IdentifierContext idContext)
        {
            string varName = idContext.ID().GetText();

            c.Comment("Assignment to variable: " + varName);

            Visit(context.expr(1));

            var valueObject = c.PopObject(Register.X0); // Pop the value to assign

            var (offset, varObject) = c.GetObject(varName); // Get the variable object

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
        var right = c.PopObject(Register.X1); // Pop 2; TOP -> [1]
        var left = c.PopObject(Register.X0); // Pop 1; TOP -> []

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

}