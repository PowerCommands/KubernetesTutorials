namespace PainKiller.PowerCommands.Core.Managers;

public class RunFlowManager
{
    public RunFlowManager(string[] args)
    {
        Args = args;
        RunAutomatedAtStartup = args.Length > 0;
    }

    public string[] Args { get;}
    public bool RunAutomatedAtStartup { get; private set; }
    public bool RunOnceThenQuit { get; private set; }
    public RunResultStatus CurrentRunResultStatus { get; set; } = RunResultStatus.Initializing;
    public string ContinueWith { get; set; } = "";
    public string Raw { get; set; } = "";

    public ICommandLineInput InitializeRunAutomation(ICommandLineInput input)
    {
        var retVal = input;
        if (!RunAutomatedAtStartup) return retVal;
        RunAutomatedAtStartup = false;
        RunOnceThenQuit = input.HasOption("justRunOnceThenQuitPowerCommand");
        if (!RunOnceThenQuit) return retVal; 
        var raw = input.Raw.Replace(" --justRunOnceThenQuitPowerCommand", "");  //Remove the option that is triggering a shutdown when application is starting up with a proxy command.
        retVal = raw.Interpret("commands");
        return retVal;
    }
}