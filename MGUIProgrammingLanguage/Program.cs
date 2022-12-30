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

    static void UpdateCode()
    {
        
    }

    static void GenerateTestProgram()
    {
        var countParam = Parameter.Output();
        mainBlock = Blocks.Assignment(countParam, 0);
        var nextBlock = mainBlock.ConnectNextBlock(Blocks.Print("START"));
        var whileLoopBlock = nextBlock.ConnectNextBlock(Blocks.WhileLoop(countParam < 5));
        {
            whileLoopBlock.Children.Add(Blocks.Assignment(countParam, countParam + 1));
            whileLoopBlock.Children.Add(Blocks.Print(Parameter.Input("Loop nr") + countParam.AsString()));
            var readParam = Parameter.Output();
            whileLoopBlock.Children.Add(Blocks.ReadInput(readParam,"Input: "));
            whileLoopBlock.Children.Add(Blocks.Print(readParam));
            whileLoopBlock.Children.Add(Blocks.Sleep(1.0f));
        }
        whileLoopBlock.ConnectNextBlock(Blocks.Print("END"));
    }
    
    static void Main(string[] args)
    {
        GenerateTestProgram();

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

    private static string _tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
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
            File.WriteAllText(_tempPath, code);
            
            StartProcess(_tempPath);
        }
    }

    private static void WindowOnClosed(object? sender, EventArgs e)
    {
        Window win = sender as Window;
        win?.Close();
    }
}