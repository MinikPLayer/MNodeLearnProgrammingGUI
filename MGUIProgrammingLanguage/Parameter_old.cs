namespace MGUIProgrammingLanguage;

public abstract class Parameter_old
{
    public bool IsInput = false;
    public string Name = "";
    public Parameter_old? InputParameter = null;
    
    protected abstract object? _GetValue();

    public object? GetValue()
    {
        if (IsInput && InputParameter != null)
            return InputParameter.GetValue();

        return _GetValue();
    }
    
    public string GetValueString()
    {
        var value = GetValue();
        if (value == null)
            return "";
        
        if (IsInput && InputParameter is null && value is string s)
            return $"\"{s}\"";

        return value.ToString()!;
    }
}

public class Parameter_old<T> : Parameter_old
{
    private T? _value { get; set; }

    protected override object? _GetValue()
    {
        return _value;
    }

    private Parameter_old(string name, bool isInput, Parameter_old? inputParameter, T value = default(T))
    {
        this.IsInput = isInput;
        this.Name = name;
        this._value = value;
        this.InputParameter = inputParameter;
    }

    public static Parameter_old<T> CreateInputParameter(string name, T value = default(T), Parameter_old? inputParameter = null) =>
        new (name, true, inputParameter, value);
    
    public static Parameter_old<T> CreateOutputParameter(string name, T value = default(T)) =>
        new (name, false, null, value);
}