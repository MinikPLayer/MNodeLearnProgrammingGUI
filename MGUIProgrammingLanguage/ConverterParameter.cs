using System.Collections;

namespace MGUIProgrammingLanguage;

public class ConverterParameter : Parameter
{
    public Parameter TargetParameter;
    
    protected override string _GetValue()
    {
        return $"{TypeName}({TargetParameter.Value})";
    }

    public ConverterParameter(string targetType, Parameter parameter)
        : base(true, typeName: targetType)
    {
        TargetParameter = parameter;
    }
}