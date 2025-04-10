

public abstract record ValueWrapper;


public record IntValue(int Value) : ValueWrapper
{
    public override string ToString() => Value.ToString();
}
public record FloatValue(float Value) : ValueWrapper
{
    public override string ToString() => Value.ToString();
}
public record StringValue(string Value) : ValueWrapper
{
    public override string ToString() => Value;
}
public record BoolValue(bool Value) : ValueWrapper
{
    public override string ToString() => Value.ToString();
}
public record VoidValue : ValueWrapper
{
    public override string ToString() => "void";
}

public record FunctionValue(Incovable invocable, string name) : ValueWrapper
{
    public override string ToString() => name;
}

public record InstanceValue(Instance instance) : ValueWrapper
{
    public override string ToString() => instance.ToString();
}

public record ClassValue(LanguageClass languageClass) : ValueWrapper
{
    public override string ToString()
    {
        return $"class {languageClass.Nombre}";
    }
}