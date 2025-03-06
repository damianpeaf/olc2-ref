public class Embeded
{
    public static void Generate(Environment env)
    {
        env.DeclareVariable("time", new FunctionValue(new TimeEmbeded(), "time"), null);
        env.DeclareVariable("print", new FunctionValue(new PrintEmbeded(), "print"), null);
    }
}

public class TimeEmbeded : Invocable
{
    public int Arity()
    {
        return 0;
    }

    public ValueWrapper Invoke(List<ValueWrapper> args, CompilerVisitor visitor)
    {
        return new StringValue(DateTime.Now.ToString());
    }
}

public class PrintEmbeded : Invocable
{
    public int Arity()
    {
        return 1;
    }

    public ValueWrapper Invoke(List<ValueWrapper> args, CompilerVisitor visitor)
    {

        var output = "";

        foreach (var arg in args)
        {
            // output += value + "\n";
            output += arg switch
            {
                IntValue i => output += i.Value.ToString() + " ",
                FloatValue f => output += f.Value.ToString() + " ",
                StringValue s => output += s.Value + " ",
                BoolValue b => output += b.Value.ToString() + " ",
                VoidValue v => output += "void" + " ",
                FunctionValue fn => output += "<fn " + fn.name + ">" + " ",
                _ => throw new SemanticError("Invalid value", null)
            };
        }
        output += "\n";

        visitor.output += output;

        return visitor.defaultVoid;
    }
}