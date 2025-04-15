

using analyzer;

public class LanguageClass : Incovable
{
    public string Nombre { get; }
    public Dictionary<string, LanguageParser.VarDclContext> Props { get; }
    public Dictionary<string, ForeignFunction> Methods { get; }

    public LanguageClass(string nombre, Dictionary<string, LanguageParser.VarDclContext> props, Dictionary<string, ForeignFunction> methods)
    {
        Nombre = nombre;
        Props = props;
        Methods = methods;
    }

    public ForeignFunction? GetMethod(string name)
    {
        if (Methods.ContainsKey(name))
        {
            return Methods[name];
        }
        return null;
    }

    public int Arity()
    {
        var constructor = GetMethod("constructor");

        if (constructor != null)
        {
            return constructor.Arity();
        }
        return 0;
    }


    public ValueWrapper Invoke(List<ValueWrapper> args, InterpreterVisitor visitor)
    {
        var newInstance = new Instance(this, instance =>
        {
            var output = "";
            output += $"{Nombre}(";
            foreach (var prop in instance.Properties)
            {
                output += $"{prop.Key}: {prop.Value}, ";
            }
            return output + ")";
        });

        // Default values
        foreach (var prop in Props)
        {
            var name = prop.Key;
            var value = prop.Value;
            if (value.expr() != null)
            {
                var defaultValue = visitor.Visit(value.expr());
                newInstance.Set(name, defaultValue);
            }
            else
            {
                newInstance.Set(name, visitor.defaultVoid);
            }
        }

        // Run constructor
        var constructor = GetMethod("constructor");
        if (constructor != null)
        {
            constructor.Bind(newInstance).Invoke(args, visitor);
        }

        return new InstanceValue(newInstance);
    }

}