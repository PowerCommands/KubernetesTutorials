using System.ComponentModel;

namespace PainKiller.PowerCommands.Shared.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class PowerCommandDesignAttribute : Attribute
{
    public string Description { get; }
    [Description("Separate items with | character, if required begin with a ! character")]
    public string Arguments { get; }
    [Description("Separate items with | character, if required begin with a ! character")]
    public string Quotes { get; }
    [Description("Separate items with | character, if required begin with a ! character")] 
    public string Options { get; }
    [Description("Separate items with | character, if required begin with a ! character")]
    public string Secrets { get; }
    [Description("The command will exexute the RunAsync instead of Run method")]
    public bool UseAsync { get; }
    [Description("This mean that the command itself will handle the --help in any way")]
    public bool OverrideHelpOption { get; }
    [Description("Separate items with |, if you begin with // the value will be displayed as an comment row in help view.")]
    public string Examples { get; }
    public string Suggestion { get; }
    [Description("If enabled another powercommands projekt could pickup the output, helpfull if your project is used with the ProxyCommand.")]
    public bool DisableProxyOutput { get; }
    [Description("Show the .")]
    public bool ShowElapsedTime { get; }
    public PowerCommandDesignAttribute(string description, bool overrideHelpOption = false, string arguments = "", string quotes = "", string example = "", string options = "", string secrets = "", string suggestion = "", bool useAsync = false, bool disableProxyOutput = false, bool showElapsedTime = false)
    {
        Description = description;
        OverrideHelpOption = overrideHelpOption;
        Arguments = arguments;
        Quotes = quotes;
        Options = options;
        Examples = example;
        Suggestion = suggestion;
        UseAsync = useAsync;
        Secrets = secrets;
        DisableProxyOutput = disableProxyOutput;
        ShowElapsedTime = showElapsedTime;
    }
}