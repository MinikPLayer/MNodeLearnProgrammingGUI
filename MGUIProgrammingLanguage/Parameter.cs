using System.Diagnostics;
using Microsoft.Scripting;

namespace MGUIProgrammingLanguage;

public class Parameter
{
    public Parameter? InputParameter = null;
    private string _value;
    private bool _isInput;
    private string _typeName;

    public string Value
    {
        set => _value = value;
        get => _isInput && InputParameter != null ? InputParameter.Value : _GetValue();
    }
    
    public string ValueString
    {
        get
        {
            var value = Value;
            return _isInput && InputParameter is null && _typeName == "String" ? $"\"{value}\"" : value;
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
        this._typeName = typeName;
    }

    public static Parameter Input<T>(T value, string typeName = "") =>
        new(true, value.ToString(), typeName: string.IsNullOrEmpty(typeName) ? GetTypeName<T>() : typeName);

    public static Parameter Input<T>(Parameter inputParameter, string typeName = "") =>
        new(true, inputParameter: inputParameter,
            typeName: string.IsNullOrEmpty(typeName) ? GetTypeName<T>() : typeName);
    
    public static Parameter Output(string symbol = "") => new(false, CreateVariableName());

    public static Parameter Operation(Parameter p1, OperationParameter.Operations op, Parameter p2) =>
        new OperationParameter(p1, op, p2);
}