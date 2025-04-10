
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
        return null;
    }
    public override Object? VisitExprStmt(LanguageParser.ExprStmtContext context)
    {
        return null;
    }
    public override Object? VisitPrintStmt(LanguageParser.PrintStmtContext context)
    {
        c.Comment("Print statement");
        c.Comment("Visiting expression");
        Visit(context.expr());
        c.Comment("Popping value to print");
        c.Pop(Register.X0); // Pop the value to print
        c.PrintInteger(Register.X0); // Call the print function

        return null;
    }
    public override Object? VisitIdentifier(LanguageParser.IdentifierContext context)
    {
        return null;
    }
    public override Object? VisitParens(LanguageParser.ParensContext context)
    {
        return null;
    }
    public override Object? VisitNegate(LanguageParser.NegateContext context)
    {
        return null;
    }
    public override Object? VisitInt(LanguageParser.IntContext context)
    {
        var value = context.INT().GetText();
        c.Comment("Constant: " + value);
        c.Mov(Register.X0, int.Parse(value));
        c.Push(Register.X0);
        return null;
    }
    public override Object? VisitMulDiv(LanguageParser.MulDivContext context)
    {
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
        c.Pop(Register.X1); // Pop 2; TOP -> [1]
        c.Pop(Register.X0); // Pop 1; TOP -> []

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

        return null;
    }
    public override Object? VisitFloat(LanguageParser.FloatContext context)
    {
        return null;
    }
    public override Object? VisitRelational(LanguageParser.RelationalContext context)
    {
        return null;
    }
    public override Object? VisitAssign(LanguageParser.AssignContext context)
    {
        return null;
    }
    public override Object? VisitEquality(LanguageParser.EqualityContext context)
    {
        return null;
    }
    public override Object? VisitBoolean(LanguageParser.BooleanContext context)
    {
        return null;
    }
    public override Object? VisitString(LanguageParser.StringContext context)
    {
        return null;
    }
    public override Object? VisitBlockStmt(LanguageParser.BlockStmtContext context)
    {
        return null;
    }
    public override Object? VisitIfStmt(LanguageParser.IfStmtContext context)
    {
        return null;
    }
    public override Object? VisitWhileStmt(LanguageParser.WhileStmtContext context)
    {
        return null;
    }
    public override Object? VisitForStmt(LanguageParser.ForStmtContext context)
    {
        return null;
    }
    public override Object? VisitBreakStmt(LanguageParser.BreakStmtContext context)
    {
        return null;
    }
    public override Object? VisitContinueStmt(LanguageParser.ContinueStmtContext context)
    {
        return null;
    }
    public override Object? VisitReturnStmt(LanguageParser.ReturnStmtContext context)
    {
        return null;
    }
    public override Object? VisitCallee(LanguageParser.CalleeContext context)
    {
        return null;
    }
    public override Object? VisitFuncDcl(LanguageParser.FuncDclContext context)
    {
        return null;
    }
    public override Object? VisitClassDcl(LanguageParser.ClassDclContext context)
    {
        return null;
    }
    public override Object? VisitInstantiation(LanguageParser.InstantiationContext context)
    {
        return null;
    }
    public override Object? VisitArray(LanguageParser.ArrayContext context)
    {
        return null;
    }

}