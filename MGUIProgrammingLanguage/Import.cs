namespace MGUIProgrammingLanguage;

public struct Import
{
    public string From;
    public string Name;

    public Import(string name, string from = "")
    {
        this.Name = name;
        this.From = from;
    }
    
    public override string ToString()
    {
        var str = "";
        if (!string.IsNullOrEmpty(From))
            str = "from " + From + " ";

        str += "import " + Name;

        return str;
    }

    public static bool operator==(Import i1, Import i2) => i1.Equals(i2);
    public static bool operator !=(Import i1, Import i2) => !i1.Equals(i2);

    public override bool Equals(object? obj) => obj is Import im && Equals(im);
    public bool Equals(Import other)
    {
        return From == other.From && Name == other.Name;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(From, Name);
    }
}