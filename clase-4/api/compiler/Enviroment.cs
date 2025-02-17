public class Environment
{

    public Dictionary<string, int> variables = new Dictionary<string, int>();
    // TODO: parent environment

    public int GetVariable(string id)
    {
        if (variables.ContainsKey(id))
        {
            return variables[id];
        }
        else
        {
            throw new Exception("Variable " + id + " not found");
        }
    }

    public void SetVariable(string id, int value)
    {
        if (variables.ContainsKey(id))
        {
            variables[id] = value;
        }
        else
        {
            variables.Add(id, value);
        }
    }

}