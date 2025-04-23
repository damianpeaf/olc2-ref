using analyzer;

public class ForeignFunction : Incovable
{

    private Environment clousure;
    private LanguageParser.FuncDclContext node;

    public ForeignFunction(LanguageParser.FuncDclContext node, Environment clousure)
    {
        this.node = node;
        this.clousure = clousure;
    }

    public int Arity()
    {
        if (node.@params() == null)
        {
            return 0;
        }

        return node.@params().ID().Length;
    }


    public ValueWrapper Invoke(List<ValueWrapper> args, InterpreterVisitor visitor)
    {
        var newEnv = new Environment(clousure);
        var beforeCallEnv = visitor.currentEnvironment;
        visitor.currentEnvironment = newEnv;

        if (node.@params() != null)
        {
            for (int i = 0; i < node.@params().ID().Length; i++)
            {
                newEnv.Declare(node.@params().ID(i).GetText(), args[i], null);
            }
        }

        try
        {
            foreach (var stmt in node.dcl())
            {
                visitor.Visit(stmt);
            }
        }
        catch (ReturnException e)
        {
            visitor.currentEnvironment = beforeCallEnv;

            return e.Value;

        }

        visitor.currentEnvironment = beforeCallEnv;
        return visitor.defaultVoid;
    }

    public ForeignFunction Bind(Instance instance)
    {
        var hiddenEnv = new Environment(clousure);
        hiddenEnv.Declare("this", new InstanceValue(instance), null);
        return new ForeignFunction(node, hiddenEnv);
    }
}