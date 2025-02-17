
using analyzer;

public class CompilerVisitor : LanguageBaseVisitor<int>
{

    public string output = "";
    private Environment currentEnvironment = new Environment();

    // VisitProgram
    public override int VisitProgram(LanguageParser.ProgramContext context)
    {
        foreach (var dcl in context.dcl())
        {
            Visit(dcl);
        }
        return 0;
    }

    // VisitVarDcl
    public override int VisitVarDcl(LanguageParser.VarDclContext context)
    {
        string id = context.ID().GetText();
        int value = Visit(context.expr());
        currentEnvironment.SetVariable(id, value);
        return 0;
    }

    // VisitExprStmt
    public override int VisitExprStmt(LanguageParser.ExprStmtContext context)
    {
        return Visit(context.expr());
    }

    // VisitPrintStmt
    public override int VisitPrintStmt(LanguageParser.PrintStmtContext context)
    {
        int value = Visit(context.expr());
        output += value + "\n";
        return 0;
    }

    // VisitIdentifier
    public override int VisitIdentifier(LanguageParser.IdentifierContext context)
    {
        string id = context.ID().GetText();
        return currentEnvironment.GetVariable(id);
    }

    // VisitParens
    public override int VisitParens(LanguageParser.ParensContext context)
    {
        return Visit(context.expr());
    }

    // VisitNegate
    public override int VisitNegate(LanguageParser.NegateContext context)
    {
        return -Visit(context.expr());
    }

    // VisitNumber
    public override int VisitNumber(LanguageParser.NumberContext context)
    {
        return int.Parse(context.GetText());
    }

    // VisitMulDiv
    public override int VisitMulDiv(LanguageParser.MulDivContext context)
    {
        int left = Visit(context.expr(0));
        int right = Visit(context.expr(1));

        return context.op.Text == "*" ? left * right : left / right;
    }

    // VisitAddSub
    public override int VisitAddSub(LanguageParser.AddSubContext context)
    {
        int left = Visit(context.GetChild(0));
        int right = Visit(context.expr(1));

        return context.op.Text == "+" ? left + right : left - right;
    }

}