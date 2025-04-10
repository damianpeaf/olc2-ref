public interface Incovable
{
    int Arity();
    ValueWrapper Invoke(List<ValueWrapper> args, InterpreterVisitor visitor);
}