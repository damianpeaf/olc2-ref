using analyzer;

class CompilerVisitor : LanguageBaseVisitor<int>
{

    public override int VisitAddSub(LanguageParser.AddSubContext context)
    {
        int left = Visit(context.expr(0));
        int right = Visit(context.expr(1));

        return context.GetChild(1).GetText() == "+" ? left + right : left - right;
    }

    public override int VisitMulDiv(LanguageParser.MulDivContext context)
    {
        int left = Visit(context.expr(0));
        int right = Visit(context.expr(1));

        return context.GetChild(1).GetText() == "*" ? left * right : left / right;
    }

    public override int VisitNumber(LanguageParser.NumberContext context)
    {
        return int.Parse(context.INT().GetText());
    }

    public override int VisitParens(LanguageParser.ParensContext context)
    {
        return Visit(context.expr());
    }

}