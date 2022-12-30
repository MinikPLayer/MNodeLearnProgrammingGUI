namespace MGUIProgrammingLanguage;

public class OperationParameter : Parameter
{
    public enum Operations
    {
        LessThan = '<',
        LessOrEqual,
        GreaterThan = '>',
        GreaterOrEqual,
        IsEqual,
        Add = '+',
        Subtract = '-',
        Multiply = '*',
        Divide = '/',
        Modulo = '%',
        Power,
    }

    public static string GetOperationString(Operations op)
    {
        return op switch
        {
            Operations.IsEqual => "==",
            Operations.LessOrEqual => "<=",
            Operations.GreaterOrEqual => ">=",
            Operations.Power => "**",
            _ => ((char)op).ToString()
        };
    }

    public Parameter Param1;
    public Parameter Param2;
    public Operations Op;

    protected override string _GetValue()
    {
        var p1 = Param1.Value;
        var p2 = Param2.Value;
        var opStr = GetOperationString(Op);
        
        return $"{p1} {opStr} {p2}";
    }

    public OperationParameter(Parameter p1, Operations op, Parameter p2) 
        : base(true)
    {
        this.Param1 = p1;
        this.Param2 = p2;
        this.Op = op;
    }
}