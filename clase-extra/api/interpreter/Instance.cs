public class Instance
{
    public LanguageClass LanguageClass;
    public Dictionary<string, ValueWrapper> Properties;

    // To String
    public Func<string> ToString;


    public Instance(LanguageClass languageClass, Func<Instance, string> toString)
    {
        LanguageClass = languageClass;
        Properties = new Dictionary<string, ValueWrapper>();
        ToString = () => toString(this);
    }

    public void Set(string name, ValueWrapper value)
    {
        Properties[name] = value;
    }

    public ValueWrapper Get(string name)
    {
        if (Properties.ContainsKey(name))
        {
            return Properties[name];
        }

        var method = LanguageClass.GetMethod(name);
        if (method != null)
        {
            return new FunctionValue(method.Bind(this), name);
        }

        throw new Exception($"Property not found: {name}");
    }
}