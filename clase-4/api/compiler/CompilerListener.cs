


using analyzer;

public class CompilerListerner : LanguageBaseListener
{

    private Stack<int> stack = new Stack<int>();

    public int GetResult()
    {
        return stack.Peek();
    }

    // ExitNumber
    public override void ExitNumber(LanguageParser.NumberContext context)
    {
        Console.WriteLine("Pushing " + context.GetText());
        stack.Push(int.Parse(context.GetText()));
    }

    // ExitMulDiv
    public override void ExitMulDiv(LanguageParser.MulDivContext context)
    {
        int right = stack.Pop();
        Console.WriteLine("Recuperando el lado derecho " + right);

        int left = stack.Pop();
        Console.WriteLine("Recuperando el lado izquierdo " + left);

        var result = context.op.Text == "*" ? left * right : left / right;

        Console.WriteLine("Realizando la operación y pusheando al stack: " + context.op.Text + " = " + result);
        stack.Push(result);

    }

    // ExitAddSub
    public override void ExitAddSub(LanguageParser.AddSubContext context)
    {
        int right = stack.Pop();
        Console.WriteLine("Recuperando el lado derecho " + right);

        int left = stack.Pop();
        Console.WriteLine("Recuperando el lado izquierdo " + left);

        var result = context.op.Text == "+" ? left + right : left - right;

        Console.WriteLine("Realizando la operación y pusheando al stack: " + context.op.Text + " = " + result);
        stack.Push(result);
    }

}