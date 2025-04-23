
using analyzer;

public class InterpreterVisitor : LanguageBaseVisitor<ValueWrapper>
{


    public ValueWrapper defaultVoid = new VoidValue();

    public string output = "";
    public Environment currentEnvironment = new Environment(null);


    public InterpreterVisitor()
    {
        Embeded.GenerateEmbeded(currentEnvironment);
    }


    // VisitProgram
    public override ValueWrapper VisitProgram(LanguageParser.ProgramContext context)
    {
        foreach (var dcl in context.dcl())
        {
            Visit(dcl);
        }
        return defaultVoid;
    }

    // VisitVarDcl
    public override ValueWrapper VisitVarDcl(LanguageParser.VarDclContext context)
    {
        string id = context.ID().GetText();
        ValueWrapper value = Visit(context.expr());
        currentEnvironment.Declare(id, value, context.Start);
        return defaultVoid;
    }

    // VisitExprStmt
    public override ValueWrapper VisitExprStmt(LanguageParser.ExprStmtContext context)
    {
        return Visit(context.expr());
    }

    // VisitPrintStmt
    public override ValueWrapper VisitPrintStmt(LanguageParser.PrintStmtContext context)
    {
        ValueWrapper value = Visit(context.expr());
        output += value.ToString();
        output += "\n";

        // Obten el token para lanzar un error:

        return defaultVoid;
    }

    // VisitIdentifier
    public override ValueWrapper VisitIdentifier(LanguageParser.IdentifierContext context)
    {
        string id = context.ID().GetText();
        return currentEnvironment.Get(id, context.Start);
    }

    // VisitParens
    public override ValueWrapper VisitParens(LanguageParser.ParensContext context)
    {
        return Visit(context.expr());
    }

    // VisitNegate
    public override ValueWrapper VisitNegate(LanguageParser.NegateContext context)
    {
        ValueWrapper value = Visit(context.expr());
        return value switch
        {
            IntValue i => new IntValue(-i.Value),
            FloatValue f => new FloatValue(-f.Value),
            _ => throw new SemanticError("Invalid operation", context.Start)
        };
    }

    // VisitInt
    public override ValueWrapper VisitInt(LanguageParser.IntContext context)
    {
        return new IntValue(int.Parse(context.INT().GetText()));
    }

    // VisitMulDiv
    public override ValueWrapper VisitMulDiv(LanguageParser.MulDivContext context)
    {
        ValueWrapper left = Visit(context.expr(0));
        ValueWrapper right = Visit(context.expr(1));
        var op = context.op.Text;

        return (left, right, op) switch
        {
            (IntValue l, IntValue r, "*") => new IntValue(l.Value * r.Value),
            (IntValue l, IntValue r, "/") => new IntValue(l.Value / r.Value),
            (FloatValue l, FloatValue r, "*") => new FloatValue(l.Value * r.Value),
            (FloatValue l, FloatValue r, "/") => new FloatValue(l.Value / r.Value),
            _ => throw new SemanticError("Invalid operation", context.Start)
        };

    }

    // VisitAddSub
    public override ValueWrapper VisitAddSub(LanguageParser.AddSubContext context)
    {
        ValueWrapper left = Visit(context.GetChild(0));
        ValueWrapper right = Visit(context.expr(1));
        var op = context.op.Text;

        return (left, right, op) switch
        {
            (IntValue l, IntValue r, "+") => new IntValue(l.Value + r.Value),
            (IntValue l, IntValue r, "-") => new IntValue(l.Value - r.Value),
            (FloatValue l, FloatValue r, "+") => new FloatValue(l.Value + r.Value),
            (FloatValue l, FloatValue r, "-") => new FloatValue(l.Value - r.Value),
            (IntValue l, FloatValue r, "+") => new FloatValue(l.Value + r.Value),
            (IntValue l, FloatValue r, "-") => new FloatValue(l.Value - r.Value),
            (FloatValue l, IntValue r, "+") => new FloatValue(l.Value + r.Value),
            (FloatValue l, IntValue r, "-") => new FloatValue(l.Value - r.Value),
            (StringValue l, StringValue r, "+") => new StringValue(l.Value + r.Value),
            (IntValue l, StringValue r, "+") => new StringValue(l.Value.ToString() + r.Value),
            (StringValue l, IntValue r, "+") => new StringValue(l.Value + r.Value.ToString()),
            _ => throw new SemanticError("Invalid operation", context.Start)
        };
    }


    // VisitFloat
    public override ValueWrapper VisitFloat(LanguageParser.FloatContext context)
    {
        return new FloatValue(float.Parse(context.FLOAT().GetText()));
    }

    // VisitRelational
    public override ValueWrapper VisitRelational(LanguageParser.RelationalContext context)
    {
        ValueWrapper left = Visit(context.expr(0));
        ValueWrapper right = Visit(context.expr(1));
        var op = context.op.Text;

        return (left, right, op) switch
        {
            (IntValue l, IntValue r, "<") => new BoolValue(l.Value < r.Value),
            (IntValue l, IntValue r, "<=") => new BoolValue(l.Value <= r.Value),
            (IntValue l, IntValue r, ">") => new BoolValue(l.Value > r.Value),
            (IntValue l, IntValue r, ">=") => new BoolValue(l.Value >= r.Value),
            (FloatValue l, FloatValue r, "<") => new BoolValue(l.Value < r.Value),
            (FloatValue l, FloatValue r, "<=") => new BoolValue(l.Value <= r.Value),
            (FloatValue l, FloatValue r, ">") => new BoolValue(l.Value > r.Value),
            (FloatValue l, FloatValue r, ">=") => new BoolValue(l.Value >= r.Value),
            _ => throw new SemanticError("Invalid operation", context.Start)
        };
    }

    // VisitAssign
    public override ValueWrapper VisitAssign(LanguageParser.AssignContext context)
    {

        var assignee = context.expr(0);
        ValueWrapper value = Visit(context.expr(1));

        if (assignee is LanguageParser.IdentifierContext idContext)
        {
            string id = idContext.ID().GetText();
            currentEnvironment.Assign(id, value, context.Start);
            return defaultVoid;
        }
        else if (assignee is LanguageParser.CalleeContext calleeContext)
        {
            // Handle property access, only last property can be assigned

            ValueWrapper calle = Visit(calleeContext.expr());

            for (int i = 0; i < calleeContext.call().Length; i++)
            {
                var action = calleeContext.call(i);

                // A this point we know that calle SHOULD be an instance
                if (i == calleeContext.call().Length - 1)
                {
                    // handle property access
                    if (action is LanguageParser.GetContext propertyAccess)
                    {
                        if (calle is InstanceValue instanceValue)
                        {
                            var instance = instanceValue.instance;
                            var property = propertyAccess.ID().GetText();
                            instance.Set(property, value);
                        }
                        else
                        {
                            throw new SemanticError("Cant assign to this expression", context.Start);
                        }
                    }
                    // TODO: handle array SET
                    else if (action is LanguageParser.ArrayAccessContext arrayAccess)
                    {
                        if (calle is InstanceValue instanceValue)
                        {
                            var instance = instanceValue.instance;
                            var index = Visit(arrayAccess.expr());

                            if (index is IntValue intVal)
                            {
                                instance.Set(intVal.Value.ToString(), value);
                            }
                            else if (index is FloatValue f)
                            {
                                instance.Set(f.Value.ToString(), value);
                            }
                            else
                            {
                                throw new SemanticError("Invalid index", context.Start);
                            }
                        }
                        else
                        {
                            throw new SemanticError("Invalid array access", context.Start);
                        }
                    }
                    else
                    {
                        throw new SemanticError("Invalid property access", context.Start);
                    }
                }

                // handle function calls
                if (action is LanguageParser.FuncCallContext funcCall)
                {
                    if (calle is FunctionValue functionValue)
                    {
                        calle = VisitCall(functionValue.invocable, funcCall.args());
                    }
                    else
                    {
                        throw new SemanticError("Invalid call", context.Start);
                    }
                }

                // Handle property access
                else if (action is LanguageParser.GetContext propertyAccess)
                {
                    if (calle is InstanceValue instanceValue)
                    {
                        var instance = instanceValue.instance;
                        var property = propertyAccess.ID().GetText();
                        calle = instance.Get(property);
                    }
                    else
                    {
                        throw new SemanticError("Invalid property access", context.Start);
                    }
                }

                // TODO: Handle array access
                else if (action is LanguageParser.ArrayAccessContext arrayAccess)
                {
                    if (calle is InstanceValue instanceValue)
                    {
                        var instance = instanceValue.instance;
                        var index = Visit(arrayAccess.expr());

                        if (index is IntValue intVal)
                        {
                            calle = instance.Get(intVal.Value.ToString());
                        }
                        else if (index is FloatValue f)
                        {
                            calle = instance.Get(f.Value.ToString());
                        }
                        else
                        {
                            throw new SemanticError("Invalid index", context.Start);
                        }
                    }
                    else
                    {
                        throw new SemanticError("Invalid array access", context.Start);
                    }
                }

            }

            return defaultVoid;
        }
        else
        {
            throw new SemanticError("Cannot assign to this expression", context.Start);
        }
    }


    // VisitEquality
    public override ValueWrapper VisitEquality(LanguageParser.EqualityContext context)
    {
        ValueWrapper left = Visit(context.expr(0));
        ValueWrapper right = Visit(context.expr(1));
        var op = context.op.Text;

        return (left, right, op) switch
        {
            (IntValue l, IntValue r, "==") => new BoolValue(l.Value == r.Value),
            (IntValue l, IntValue r, "!=") => new BoolValue(l.Value != r.Value),
            (FloatValue l, FloatValue r, "==") => new BoolValue(l.Value == r.Value),
            (FloatValue l, FloatValue r, "!=") => new BoolValue(l.Value != r.Value),
            (StringValue l, StringValue r, "==") => new BoolValue(l.Value == r.Value),
            (StringValue l, StringValue r, "!=") => new BoolValue(l.Value != r.Value),
            (BoolValue l, BoolValue r, "==") => new BoolValue(l.Value == r.Value),
            (BoolValue l, BoolValue r, "!=") => new BoolValue(l.Value != r.Value),
            _ => throw new SemanticError("Invalid operation", context.Start)
        };
    }


    // VisitBoolean
    public override ValueWrapper VisitBoolean(LanguageParser.BooleanContext context)
    {
        return new BoolValue(bool.Parse(context.BOOL().GetText()));
    }


    // VisitString
    public override ValueWrapper VisitString(LanguageParser.StringContext context)
    {
        return new StringValue(context.STRING().GetText());
    }


    // VisitBlockStmt
    public override ValueWrapper VisitBlockStmt(LanguageParser.BlockStmtContext context)
    {
        Environment previousEnvironment = currentEnvironment;
        currentEnvironment = new Environment(currentEnvironment);

        foreach (var stmt in context.dcl())
        {
            Visit(stmt);
        }

        currentEnvironment = previousEnvironment;
        return defaultVoid;
    }

    // VisitIfStmt
    public override ValueWrapper VisitIfStmt(LanguageParser.IfStmtContext context)
    {
        ValueWrapper condition = Visit(context.expr());

        if (condition is not BoolValue)
        {
            throw new SemanticError("Invalid condition", context.Start);
        }

        if (condition is BoolValue boolCondition && boolCondition.Value)
        {
            Visit(context.stmt(0));
        }
        else if (context.stmt().Length > 1)
        {
            Visit(context.stmt(1));
        }

        return defaultVoid;
    }

    // VisitWhileStmt
    public override ValueWrapper VisitWhileStmt(LanguageParser.WhileStmtContext context)
    {
        ValueWrapper condition = Visit(context.expr());
        Environment initialEnvironment = currentEnvironment;

        if (condition is not BoolValue)
        {
            throw new SemanticError("Invalid condition", context.Start);
        }

        try
        {
            while (condition is BoolValue boolCondition && boolCondition.Value)
            {
                Visit(context.stmt());
                condition = Visit(context.expr());
            }
        }
        catch (BreakException)
        {
            currentEnvironment = initialEnvironment;
        }
        catch (ContinueException)
        {
            currentEnvironment = initialEnvironment;
            Visit(context);
        }

        return defaultVoid;
    }

    // !!!!!!!!!! -------------------- AQUI CONTINUA EL EJEMPLO ---------------------- !!!!!!!!!!

    // VisitForStmt
    public override ValueWrapper VisitForStmt(LanguageParser.ForStmtContext context)
    {

        Environment previousEnvironment = currentEnvironment;
        currentEnvironment = new Environment(currentEnvironment);

        Visit(context.forInit());

        VisitInnerFor(context);

        currentEnvironment = previousEnvironment;

        return defaultVoid;
    }

    public void VisitInnerFor(LanguageParser.ForStmtContext context)
    {
        ValueWrapper condition = Visit(context.expr(0));
        Environment initialEnvironment = currentEnvironment;

        if (condition is not BoolValue)
        {
            throw new SemanticError("Invalid condition", context.Start);
        }

        try
        {

            while (condition is BoolValue boolCondition && boolCondition.Value)
            {
                Visit(context.stmt());
                Visit(context.expr(1)); // Update
                condition = Visit(context.expr(0)); // Condition
            }
        }
        catch (BreakException)
        {
            currentEnvironment = initialEnvironment;
        }
        catch (ContinueException)
        {
            currentEnvironment = initialEnvironment;
            Visit(context.expr(1));
            VisitInnerFor(context);
        }

    }


    // VisitBreakStmt
    public override ValueWrapper VisitBreakStmt(LanguageParser.BreakStmtContext context)
    {
        throw new BreakException();
    }

    // VisitContinueStmt
    public override ValueWrapper VisitContinueStmt(LanguageParser.ContinueStmtContext context)
    {
        throw new ContinueException();
    }

    // VisitReturnStmt
    public override ValueWrapper VisitReturnStmt(LanguageParser.ReturnStmtContext context)
    {
        ValueWrapper value = defaultVoid;
        if (context.expr() != null)
        {
            value = Visit(context.expr());
        }
        throw new ReturnException(value);
    }

    // VisitCall
    public override ValueWrapper VisitCallee(LanguageParser.CalleeContext context)
    {
        ValueWrapper calle = Visit(context.expr());

        foreach (var action in context.call())
        {

            // handle function calls
            if (action is LanguageParser.FuncCallContext funcCall)
            {
                if (calle is FunctionValue functionValue)
                {
                    calle = VisitCall(functionValue.invocable, funcCall.args());
                }
                else
                {
                    throw new SemanticError("Invalid call", context.Start);
                }
            }

            // Handle property access
            else if (action is LanguageParser.GetContext propertyAccess)
            {
                if (calle is InstanceValue instanceValue)
                {
                    var instance = instanceValue.instance;
                    var property = propertyAccess.ID().GetText();
                    calle = instance.Get(property);
                }
                else
                {
                    throw new SemanticError("Invalid property access", context.Start);
                }
            }

            else if (action is LanguageParser.ArrayAccessContext arrayAccess)
            {
                // TODO:
                if (calle is InstanceValue instanceValue)
                {
                    var instance = instanceValue.instance;
                    var index = Visit(arrayAccess.expr());

                    if (index is IntValue i)
                    {
                        calle = instance.Get(i.Value.ToString());
                    }
                    else if (index is FloatValue f)
                    {
                        calle = instance.Get(f.Value.ToString());
                    }
                    else
                    {
                        throw new SemanticError("Invalid index", context.Start);
                    }
                }
                else
                {
                    throw new SemanticError("Invalid array access", context.Start);
                }
            }

        }

        return calle;
    }

    public ValueWrapper VisitCall(Incovable invocable, LanguageParser.ArgsContext context)
    {
        List<ValueWrapper> arguments = new List<ValueWrapper>();

        if (context != null)
        {
            foreach (var arg in context.expr())
            {
                arguments.Add(Visit(arg));
            }
        }

        if (context != null && arguments.Count != invocable.Arity())
        {
            throw new SemanticError("Invalid number of arguments", context.Start);
        }

        var result = invocable.Invoke(arguments, this);

        return result;
    }

    // VisitFuncDcl
    public override ValueWrapper VisitFuncDcl(LanguageParser.FuncDclContext context)
    {
        var foreignFunction = new ForeignFunction(context, currentEnvironment);
        currentEnvironment.Declare(context.ID().GetText(), new FunctionValue(foreignFunction, context.ID().GetText()), context.Start);
        return defaultVoid;
    }


    // VisitClassDcl
    public override ValueWrapper VisitClassDcl(LanguageParser.ClassDclContext context)
    {
        Dictionary<string, LanguageParser.VarDclContext> props = new Dictionary<string, LanguageParser.VarDclContext>();
        Dictionary<string, ForeignFunction> methods = new Dictionary<string, ForeignFunction>();

        foreach (var prop in context.classBody())
        {
            if (prop.varDcl() != null)
            {
                var varDcl = prop.varDcl();
                props.Add(varDcl.ID().GetText(), varDcl);
            }
            else if (prop.funcDcl() != null)
            {
                var funcDcl = prop.funcDcl();
                var foreignFunction = new ForeignFunction(funcDcl, currentEnvironment);
                methods.Add(funcDcl.ID().GetText(), foreignFunction);
            }
        }

        LanguageClass languageClass = new LanguageClass(context.ID().GetText(), props, methods);

        currentEnvironment.Declare(context.ID().GetText(), new ClassValue(languageClass), context.Start);

        return defaultVoid;
    }

    // VisitInstantiation
    public override ValueWrapper VisitInstantiation(LanguageParser.InstantiationContext context)
    {
        ValueWrapper classValue = currentEnvironment.Get(context.ID().GetText(), context.Start);
        if (classValue is not ClassValue)
        {
            throw new SemanticError("Invalid class", context.Start);
        }

        List<ValueWrapper> arguments = new List<ValueWrapper>();
        if (context.args() != null)
        {
            foreach (var arg in context.args().expr())
            {
                arguments.Add(Visit(arg));
            }
        }

        var instanceValue = ((ClassValue)classValue).languageClass.Invoke(arguments, this);

        return instanceValue;
    }

    // VisitArray
    public override ValueWrapper VisitArray(LanguageParser.ArrayContext context)
    {
        List<ValueWrapper> arguments = new List<ValueWrapper>();
        if (context.args() != null)
        {
            foreach (var arg in context.args().expr())
            {
                arguments.Add(Visit(arg));
            }
        }

        var arrayClass = new LanguageArray();
        var instanceValue = arrayClass.Invoke(arguments, this);

        return instanceValue;
    }

}