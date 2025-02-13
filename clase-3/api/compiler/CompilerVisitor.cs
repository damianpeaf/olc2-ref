
using analyzer;

public class CompilerVisitor : LanguageBaseVisitor<int>
{

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