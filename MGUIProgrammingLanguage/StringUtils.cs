namespace MGUIProgrammingLanguage;

public static class StringUtils
{
    public static bool IsCharAllowedInVariableName(char c)
    {
        return (c >= 'a' && c <= 'z') || (c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || c == '_';
    }
    
    /// <summary>
    /// Function replaces match string with replaceWith, but only if string matches variables rules
    /// ( Using "Test $var10 test" as data string and "$var1" as match - nothing will happen, but "$var10" will be replaced )
    /// </summary>
    /// <param name="ogData">Original data string</param>
    /// <param name="match">Replace match</param>
    /// <param name="replaceWith">Data to replace with</param>
    /// <returns>Result of the operation</returns>
    public static string ReplaceVariable(this string ogData, string match, string replaceWith)
    {
        var matchStart = 0;
        var data = ogData;
        for (var i = 0; i < data.Length; i++)
        {
            var diff = i - matchStart;
            if (diff >= match.Length || data[i] != match[diff])
            {
                matchStart = i + 1;
            }
            else if (diff + 1 == match.Length && (data.Length == i + 1 || !IsCharAllowedInVariableName(data[i + 1])))
            {
                // Replace
                data = data.Remove(matchStart, match.Length).Insert(matchStart, replaceWith);
                i -= match.Length - replaceWith.Length;
                matchStart = i + 1;
            }
        }

        return data;
    }
}