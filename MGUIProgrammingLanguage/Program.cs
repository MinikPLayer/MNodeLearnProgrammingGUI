using System.Diagnostics;
using IronPython.Hosting;
using MGUIProgrammingLanguage;
using Microsoft.Scripting.Hosting;
using SFML.Graphics;
using SFML.Window;

static class Program
{
    private static CodeBlock? mainBlock = null;
    
    static string GetCode()
    {
        //return "import time\nt=1\nwhile True:\n\tprint(t)\n\tt=t+1\n\ttime.sleep(1)";

        var imports = new HashSet<Import>();
        
        var ret = "";
        var block = mainBlock;
        while (block != null)
        {
            ret += block.GetCode() + "\n";
            var newImports = block.GetRequiredImports();
            foreach (var import in newImports)
                imports.Add(import);
            
            block = block.NextBlock;
        }

        var importsStr = imports.Aggregate("", (current, import) => current + (import + "\n"));
        return importsStr + "\n" + ret;
    }
    
    static void Main(string[] args)
    {
        // mainBlock = new CodeBlock("Read user input", "$out1 = input($var1)",
        //     new List<Parameter>
        //     {
        //         Parameter<string>.CreateInputParameter("$var1", "Input:"),
        //         Parameter<string>.CreateOutputParameter($"$out1", "out")
        //     }
        // );
        //
        // mainBlock.ConnectNextBlock(new CodeBlock("Print Hello world", "print($var1)",
        //     new List<Parameter> { Parameter<string>.CreateInputParameter("$var1", "Hello world!", mainBlock.parameters[1]) }));
        //

        // mainBlock = BlocksDefinitions.WhileLoopBlock(Parameter<bool>.CreateInputParameter("", true));
        // {
        //     var readParam = Parameter<string>.CreateOutputParameter("$var", "out");
        //     mainBlock.children.Add(BlocksDefinitions.ReadInputBlock(readParam, Parameter<string>.CreateInputParameter("", "Enter your name: ")));
        //     mainBlock.children.Add(BlocksDefinitions.PrintBlock(Parameter<string>.CreateInputParameter("", "", inputParameter: readParam)));
        // }
        
        var readParam = Parameter.Output();
        mainBlock = BlocksDefinitions.Assignment(readParam, Parameter.Input(0));
        var whileLoopBlock = mainBlock.ConnectNextBlock(BlocksDefinitions.WhileLoop(
            Parameter.Operation(readParam, OperationParameter.Operations.LessThan, Parameter.Input(5))));
        {
            whileLoopBlock.Children.Add(BlocksDefinitions.Assignment(readParam,
                Parameter.Operation(readParam, OperationParameter.Operations.Add, Parameter.Input(1))));
            whileLoopBlock.Children.Add(BlocksDefinitions.Print(readParam));
            whileLoopBlock.Children.Add(BlocksDefinitions.Sleep(Parameter.Input(1.0)));
        }


        var window = new RenderWindow(new VideoMode(1280, 720), "MGUI");
        window.Closed += WindowOnClosed;
        window.KeyPressed += WindowOnKeyPressed;
        window.SetVerticalSyncEnabled(true);
        
        while (window.IsOpen)
        {
            window.DispatchEvents();

            window.Clear(Color.Black);
            foreach (var b in CodeBlock.blocksList)
                window.Draw(b);
            window.Display();
        }
        
        KillProcess();
    }

    private static Process? _pythonProcess = null;
    static void KillProcess()
    {
        _pythonProcess?.Kill();
        Console.WriteLine("Process killed");
    }

    static string GetPythonPath()
    {
        var path = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.User);
        if (path == null)
            throw new NotSupportedException("Cannot find python, because PATH doesn't exists");
        
        var paths = path.Split(';');
        foreach (var p in paths)
        {
            var combinedPath = Path.Combine(p, "python.exe");
            if(!File.Exists(combinedPath))
                continue;
            
            return combinedPath;
        }

        throw new FileNotFoundException("Cannot find python in path");
    }
    
    static void StartProcess(string path)
    {
        var pythonPath = GetPythonPath();
        _pythonProcess = Process.Start(pythonPath, path);
    }
    
    private static void WindowOnKeyPressed(object? sender, KeyEventArgs e)
    {
        if (e.Code == Keyboard.Key.P)
            Console.WriteLine(GetCode());

        if(e.Code == Keyboard.Key.C)
            KillProcess();

        if (e.Code == Keyboard.Key.R)
        {
            KillProcess();

            var code = GetCode();
            Console.WriteLine("Executing code: \n" + code);
            var tempDir = Path.GetTempPath();
            var tempPath = Path.Combine(tempDir, new Guid().ToString());
            File.WriteAllText(tempPath, code);
            
            StartProcess(tempPath);
        }
    }

    private static void WindowOnClosed(object? sender, EventArgs e)
    {
        Window win = sender as Window;
        win?.Close();
    }
}