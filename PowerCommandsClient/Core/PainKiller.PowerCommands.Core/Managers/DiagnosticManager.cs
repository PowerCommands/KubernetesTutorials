using System.Diagnostics;
using System.Reflection;

namespace PainKiller.PowerCommands.Core.Managers;

public class DiagnosticManager : IDiagnosticManager
{    
    private readonly Stopwatch _stopWatch = new();

    public DiagnosticManager(bool showDiagnostic, bool showElapsedTime = false)
    {
        ShowDiagnostic = showDiagnostic;
        ShowElapsedTime = showElapsedTime;
    }
    public bool ShowDiagnostic { get; set; }
    public bool ShowElapsedTime { get; set; }

    public void Message(string diagnostic)
    {
        if (!ShowDiagnostic) return;
        ConsoleService.Service.WriteLine(GetType().Name, diagnostic, null);
    }

    public void Header(string header)
    {
        if (!ShowDiagnostic) return;
        ConsoleService.Service.WriteHeaderLine(GetType().Name, header);
    }

    public void Warning(string warning)
    {
        if (!ShowDiagnostic) return;
        ConsoleService.Service.WriteWarning(GetType().Name, warning);
    }
    public void Start()
    {
        _stopWatch.Start();
    }
    public void Stop()
    {
        if (!ShowDiagnostic && !ShowElapsedTime)
        {
            _stopWatch.Reset();
            return;
        }
        var ts = _stopWatch.Elapsed;
        _stopWatch.Reset();
        var elapsedTime = $"{ts.Hours:00}hh:{ts.Minutes:00}mm:{ts.Seconds:00}ss.{ts.Milliseconds / 10:00}ms";
        Console.WriteLine($"\nRunTime:{elapsedTime}\n");
    }
    public string RootPath()
    {
        var retVal = Assembly.GetEntryAssembly()?.Location ?? "";
        retVal = retVal.Replace($"{Assembly.GetEntryAssembly()?.GetName(false).Name}.dll", "");
        return retVal;
    }
}