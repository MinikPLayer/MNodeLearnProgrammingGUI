using SFML.Graphics;
using SFML.System;

namespace MGUIProgrammingLanguage;

public class CodeBlock : Drawable
{
    public static List<CodeBlock> blocksList = new List<CodeBlock>();
    
    private Image img;
    private Texture txt;
    private Sprite sprite;

    public Color Color
    {
        get => sprite.Color;
        set => sprite.Color = value;
    }

    public Vector2f Position
    {
        get => sprite.Position;
        set => sprite.Position = value;
    }

    private string _code;

    public string Name;
    private List<Parameter> _parameters;
    public CodeBlock? NextBlock;

    public List<CodeBlock> Children = new();
    public List<Import> Imports;

    /// <summary>
    /// Returns imports required by this block, including it's children
    /// </summary>
    /// <returns>List of required imports</returns>
    public List<Import> GetRequiredImports()
    {
        var imports = new List<Import>(Imports);
        foreach (var c in Children)
            imports.AddRange(c.GetRequiredImports());

        return imports;
    }
    
    public string GetCode(string prefix = "")
    {
        var ret = _code;
        for (var i = 0; i < _parameters.Count; i++)
            ret = ret.ReplaceVariable($"$p{i}", _parameters[i].ValueString);

        ret = prefix + ret.Replace("\n", "\n" + prefix);
        return Children.Aggregate(ret, (current, c) => current + ("\n" + c.GetCode(prefix + "\t")));
    }

    public CodeBlock ConnectNextBlock(CodeBlock block)
    {
        NextBlock = block;
        return NextBlock;
    }

    public void Draw(RenderTarget target, RenderStates states)
    {
        target.Draw(sprite);
    }
    
    public CodeBlock(string name, string code, List<Parameter> parameters, List<Import>? imports, Color color, Vector2i size)
    {
        this.Name = name;
        this._code = code;
        this._parameters = parameters;

        imports ??= new List<Import>();
        this.Imports = new List<Import>(imports);
        
        img = new Image((uint)size.X, (uint)size.Y);
        for(uint x = 0; x < size.X; x++)
            for(uint y = 0; y < size.Y; y++)
                img.SetPixel(x, y, Color.White);
        
        txt = new Texture((uint)size.X, (uint)size.Y);
        txt.Update(img);
        sprite = new Sprite(txt);

        Color = color;
        
        blocksList.Add(this);
    }
    
    public CodeBlock(string name, string code, List<Parameter> parameters, List<Import>? imports = null) 
        : this(name, code, parameters, imports, SFML.Graphics.Color.Blue, new Vector2i(100, 100)) {}

    ~CodeBlock()
    {
        blocksList.Remove(this);
    }
}