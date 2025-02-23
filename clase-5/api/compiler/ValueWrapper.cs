

public abstract record ValueWrapper;


public record IntValue(int Value) : ValueWrapper;
public record FloatValue(float Value) : ValueWrapper;
public record StringValue(string Value) : ValueWrapper;
public record BoolValue(bool Value) : ValueWrapper;
public record VoidValue : ValueWrapper;


// public class ValueWp
// {

//     public enum ValueType
//     {
//         Int,
//         Float,
//         String,
//         Bool,
//         Void
//     }

//     public ValueType Type { get; }
//     public object Value { get; }
// }