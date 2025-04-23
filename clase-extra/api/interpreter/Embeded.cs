

public class Embeded
{

    public static void GenerateEmbeded(Environment environment)
    {

        // * time
        environment.Declare("time", new FunctionValue(new TimeEmbeded(), "<fn time>"), null);

    }

}

public class TimeEmbeded : Incovable
{
    public int Arity()
    {
        return 0;
    }

    public ValueWrapper Invoke(List<ValueWrapper> args, InterpreterVisitor visitor)
    {
        return new StringValue(DateTime.Now.ToString());
    }
}