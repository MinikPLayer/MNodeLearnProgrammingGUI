namespace MGUIProgrammingLanguage;

public static class ParameterExtensions
{
    
    public static ConverterParameter As(this Parameter p, string type) => new(type, p);
    public static ConverterParameter AsString(this Parameter p) => new("str", p);
    public static ConverterParameter AsInt(this Parameter p) => new("int", p);
    public static ConverterParameter AsFloat(this Parameter p) => new("float", p);
    public static ConverterParameter AsTuple(this Parameter p) => new("tuple", p);
    public static ConverterParameter AsSet(this Parameter p) => new("set", p);
    public static ConverterParameter AsDictionary(this Parameter p) => new("dict", p);
    
    public static OperationParameter Add(this Parameter p, Parameter p2) =>
        new(p, OperationParameter.Operations.Add, p2);

    public static OperationParameter Subtract(this Parameter p, Parameter p2) =>
        new(p, OperationParameter.Operations.Subtract, p2);

    public static OperationParameter Multiply(this Parameter p, Parameter p2) =>
        new(p, OperationParameter.Operations.Multiply, p2);
    
    public static OperationParameter Divide(this Parameter p, Parameter p2) =>
        new(p, OperationParameter.Operations.Divide, p2);
    
    public static OperationParameter Modulo(this Parameter p, Parameter p2) =>
        new(p, OperationParameter.Operations.Modulo, p2);
    
    public static OperationParameter Power(this Parameter p, Parameter p2) =>
        new(p, OperationParameter.Operations.Power, p2);
    
    public static OperationParameter LessThan(this Parameter p, Parameter p2) =>
        new(p, OperationParameter.Operations.LessThan, p2);
    
    public static OperationParameter LessOrEqual(this Parameter p, Parameter p2) =>
        new(p, OperationParameter.Operations.LessOrEqual, p2);
    
    public static OperationParameter GreaterThan(this Parameter p, Parameter p2) =>
        new(p, OperationParameter.Operations.GreaterThan, p2);
    
    public static OperationParameter GreaterOrEqual(this Parameter p, Parameter p2) =>
        new(p, OperationParameter.Operations.GreaterOrEqual, p2);
    
    public static OperationParameter IsEqualOperation(this Parameter p, Parameter p2) =>
        new(p, OperationParameter.Operations.IsEqual, p2);
}