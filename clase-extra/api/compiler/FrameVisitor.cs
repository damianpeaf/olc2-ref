

using analyzer;

public class FrameElement
{
    public string Name { get; set; }
    public int Offset { get; set; }

    public FrameElement(string name, int offset)
    {
        Name = name;
        Offset = offset;
    }
}

public class FrameVisitor : LanguageBaseVisitor<Object?>
{

    public List<FrameElement> Frame;
    public int LocalOffset;
    public int BaseOffset;

    public FrameVisitor(int baseOffset)
    {
        Frame = new List<FrameElement>();
        LocalOffset = 0;
        BaseOffset = baseOffset;
    }


    public override Object? VisitVarDcl(LanguageParser.VarDclContext context)
    {
        string name = context.ID().GetText();

        Frame.Add(new FrameElement(name, BaseOffset + LocalOffset));
        LocalOffset += 1;

        return null;
    }

    public override Object? VisitBlockStmt(LanguageParser.BlockStmtContext context)
    {

        foreach (var dcl in context.dcl())
        {
            Visit(dcl);
        }

        return null;
    }

    public override Object? VisitIfStmt(LanguageParser.IfStmtContext context)
    {

        Visit(context.stmt(0));
        if (context.stmt().Length > 1) Visit(context.stmt(1));

        return null;
    }

    public override Object? VisitWhileStmt(LanguageParser.WhileStmtContext context)
    {
        Visit(context.stmt());
        return null;
    }

    public override Object? VisitForStmt(LanguageParser.ForStmtContext context)
    {

        if (context.forInit().varDcl() != null)
        {
            Visit(context.forInit().varDcl());
        }

        Visit(context.stmt());

        return null;
    }

}