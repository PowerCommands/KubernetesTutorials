using System.ComponentModel;

namespace PainKiller.PowerCommands.Shared.Attributes
{
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
        public string Suggestions { get; }
        [Description("If enabled another powercommands projekt could pickup the output, helpfull if your project is used with the ProxyCommand.")]
        public bool DisableProxyOutput { get; }
        [Description("Show the .")]
        public bool ShowElapsedTime { get; }
        /// <summary>
        /// Set design parameters to help the consumer and control the program flow (async, diagnostics and validation)
        /// </summary>
        /// <param name="description"></param>
        /// <param name="overrideHelpOption">If you set this to true the command needs to handle the display of help, the default one will not be shown.</param>
        /// <param name="arguments"></param>
        /// <param name="quotes"></param>
        /// <param name="example">Show example usage, separate examples with |, show comments with a beginning //</param>
        /// <param name="options">Options flag names separated with |</param>
        /// <param name="secrets">Secrets that has to be set in config separated with |</param>
        /// <param name="suggestions">Suggestion to the command completion separated with |</param>
        /// <param name="useAsync"></param>
        /// <param name="disableProxyOutput"></param>
        /// <param name="showElapsedTime">Show diagnostic elapsed time info</param>
        public PowerCommandDesignAttribute(string description, bool overrideHelpOption = false, string arguments = "", string quotes = "", string example = "", string options = "", string secrets = "", string suggestions = "", bool useAsync = false, bool disableProxyOutput = false, bool showElapsedTime = false)
        {
            Description = description;
            OverrideHelpOption = overrideHelpOption;
            Arguments = arguments;
            Quotes = quotes;
            Options = options;
            Examples = example;
            Suggestions = suggestions;
            UseAsync = useAsync;
            Secrets = secrets;
            DisableProxyOutput = disableProxyOutput;
            ShowElapsedTime = showElapsedTime;
        }
    }
}