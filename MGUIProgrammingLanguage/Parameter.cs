using System.Diagnostics;
using Microsoft.Scripting;

namespace MGUIProgrammingLanguage;

public class Parameter
{
    public Parameter? InputParameter = null;
    private string _value;
    private bool _isInput;
    protected string TypeName;

    public string Value
    {
        set => _value = value;
        get
        {
            if (!_isInput) 
                return _value;

            if (InputParameter != null) 
                return InputParameter.Value;
            
            var value = _GetValue();
            return TypeName == "String" ? $"\"{value}\"" : value;
        }
    }

    protected virtual string _GetValue() => _value;

    private static int _variableCounter = 0;
    // TODO: Change to fix $var1 being interpreted the same as $var10
    static string CreateVariableName() => $"var{_variableCounter++}";

    private static string GetTypeName<T>()
    {
        return typeof(T).Name;
    }

    protected Parameter(bool isInput, string value = "", Parameter? inputParameter = null, string typeName = "")
    {
        this._value = value;
        this.InputParameter = inputParameter;
        this._isInput = isInput;
        this.TypeName = typeName;
    }

    public static Parameter Input<T>(T value, string typeName = "") =>
        new(true, value.ToString(), typeName: string.IsNullOrEmpty(typeName) ? GetTypeName<T>() : typeName);

    public static Parameter Input<T>(Parameter inputParameter, string typeName = "") =>
        new(true, inputParameter: inputParameter,
            typeName: string.IsNullOrEmpty(typeName) ? GetTypeName<T>() : typeName);
    
    public static Parameter Output(string symbol = "") => new(false, CreateVariableName());

    public static Parameter Operation(Parameter p1, OperationParameter.Operations op, Parameter p2) =>
        new OperationParameter(p1, op, p2);
    
    
    public static Parameter operator +(Parameter p1, Parameter p2) => p1.Add(p2);
    public static Parameter operator -(Parameter p1, Parameter p2) => p1.Subtract(p2);
    public static Parameter operator *(Parameter p1, Parameter p2) => p1.Multiply(p2);
    public static Parameter operator /(Parameter p1, Parameter p2) => p1.Divide(p2);
    public static Parameter operator >(Parameter p1, Parameter p2) => p1.GreaterThan(p2);
    public static Parameter operator >=(Parameter p1, Parameter p2) => p1.GreaterOrEqual(p2);
    public static Parameter operator <(Parameter p1, Parameter p2) => p1.LessThan(p2);
    public static Parameter operator <=(Parameter p1, Parameter p2) => p1.LessOrEqual(p2);
    public static Parameter operator %(Parameter p1, Parameter p2) => p1.Modulo(p2);

    public static implicit operator Parameter(string value) => Input(value);
    public static implicit operator Parameter(int value) => Input(value);
    public static implicit operator Parameter(float value) => Input(value);
}