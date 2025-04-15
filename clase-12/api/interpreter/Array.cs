



using analyzer;

public class LanguageArray : Incovable
{
    public Dictionary<string, LanguageParser.VarDclContext> Props { get; }
    public Dictionary<string, ForeignFunction> Methods { get; }

    private LanguageClass equivalentClass;

    public LanguageArray()
    {
        Props = new Dictionary<string, LanguageParser.VarDclContext>();
        Methods = new Dictionary<string, ForeignFunction>();
        equivalentClass = new LanguageClass("[]", Props, Methods);
    }


    public int Arity()
    {
        return 100;
    }


    public ValueWrapper Invoke(List<ValueWrapper> args, InterpreterVisitor visitor)
    {
        var newInstance = new Instance(equivalentClass, instance =>
        {
            var output = "";
            output += $"[";
            foreach (var prop in instance.Properties)
            {
                output += $"{prop.Value},";
            }
            if (output.Length > 1)
            {
                output = output.TrimEnd(',');
            }
            return output + "]";
        });

        // Default values
        for (int i = 0; i < args.Count; i++)
        {
            var name = i.ToString();
            var value = args[i];
            newInstance.Set(name, value);
        }

        // Run constructor
        return new InstanceValue(newInstance);
    }

}