namespace MGUIProgrammingLanguage;

public static class Blocks
{
    public static CodeBlock Print(Parameter printData)
    {
        return new CodeBlock("Print", "print($p0)", new List<Parameter> { printData });
    } 

    public static CodeBlock ReadInput(Parameter outputParameter, Parameter? inputHintParameter = null)
    {
        inputHintParameter ??= Parameter.Input("");
        return new CodeBlock("Read input", "$p0 = input($p1)", new List<Parameter> { outputParameter, inputHintParameter });
    }

    public static CodeBlock WhileLoop(Parameter conditionParameter)
    {
        return new CodeBlock("While loop", $"while $p0:", new List<Parameter> { conditionParameter });
    }

    public static CodeBlock Assignment(Parameter outParam, Parameter inputParam)
    {
        return new CodeBlock("Assignment", "$p0 = $p1", new List<Parameter> { outParam, inputParam });
    }

    public static CodeBlock Sleep(Parameter sleepTime)
    {
        return new CodeBlock("Sleep", "time.sleep($p0)", new List<Parameter> { sleepTime },
            new List<Import> { new("time") });
    }
}