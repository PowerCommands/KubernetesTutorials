using Microsoft.Extensions.Logging;
using System.Text;

namespace PainKiller.PowerCommands.Core.BaseClasses
{
    public abstract class CommandBase<TConfig> : IConsoleCommand, IConsoleWriter where TConfig : new()
    {
        private IConsoleService _console;
        protected ICommandLineInput Input = new CommandLineInput();
        protected List<PowerOption> Options = new();
        private readonly StringBuilder _ouput = new();
        protected string LastReadLine = "";
        protected CommandBase(string identifier, TConfig configuration, IConsoleService? console = null)
        {
            Identifier = identifier;
            Configuration = configuration;
            _console = console ?? ConsoleService.Service;
        }
        protected virtual void ConsoleWriteToOutput(string output)
        {
            if (AppendToOutput) _ouput.Append(output);
        }
        public string Identifier { get; }
        protected bool AppendToOutput { get; set; } = true;
        protected PowerCommandDesignAttribute? DesignAttribute { get; private set; }
        public virtual bool InitializeAndValidateInput(ICommandLineInput input, PowerCommandDesignAttribute? designAttribute = null)
        {
            Options.Clear();
            designAttribute ??= new PowerCommandDesignAttribute("This command has no design attribute");
            if (IPowerCommandServices.DefaultInstance!.DefaultConsoleService.GetType().Name != _console.GetType().Name) _console = IPowerCommandServices.DefaultInstance.DefaultConsoleService;
            Input = input;
            DesignAttribute = designAttribute;
            IPowerCommandServices.DefaultInstance.Diagnostic.ShowElapsedTime = DesignAttribute.ShowElapsedTime;
            var validationManager = new InputValidationManager(this, input, WriteError);
            var result = validationManager.ValidateAndInitialize();
            if (result.Options.Count > 0) Options.AddRange(result.Options);
            _console.WriteToOutput += ConsoleWriteToOutput;
            return result.HasValidationError;
        }
        public virtual void RunCompleted()
        {
            _console.WriteToOutput -= ConsoleWriteToOutput;
            if (IPowerCommandServices.DefaultInstance!.DefaultConsoleService.GetType().Name != ConsoleService.Service.GetType().Name) Console.WriteLine(_ouput.ToString());
            _ouput.Clear();
        }
        public virtual RunResult Run() => throw new NotImplementedException();
        public virtual async Task<RunResult> RunAsync() => await Task.FromResult(new RunResult(this, Input, "", RunResultStatus.Initializing));
        protected TConfig Configuration { get; set; }

        /// <summary>
        /// Disable log of severity levels Trace,Debug and Information.
        /// </summary>
        protected void DisableLog() => ConsoleService.Service.DisableLog();
        protected void EnableLog() => ConsoleService.Service.EnableLog();

        #region Options
        protected string GetOptionValue(string optionName) => Input.GetOptionValue(optionName);
        protected void DoBadOptionCheck() => Input.DoBadOptionCheck(this);
        protected bool HasOption(string optionName) => Input.HasOption(optionName);
        protected string FirstOptionWithValue() => Input.FirstOptionWithValue();
        protected bool MustHaveOneOfTheseOptionCheck(string[] optionNames) => Input.MustHaveOneOfTheseOptionCheck(optionNames);
        protected bool NoOption(string optionName) => Input.NoOption(optionName);
        #endregion

        #region output
        protected void DisableOutput() => AppendToOutput = false;
        protected void EnableOutput() => AppendToOutput = true;
        protected void DisplayOutput() => Console.WriteLine(_ouput.ToString());
        protected void ClearOutput() => _ouput.Clear();
        protected bool FindInOutput(string findPhrase) => _ouput.ToString().Contains(findPhrase);
        #endregion

        #region RunResult
        protected RunResult Ok() => new(this, Input, _ouput.ToString(), RunResultStatus.Ok);
        protected RunResult Quit() => new(this, Input, _ouput.ToString(), RunResultStatus.Quit);
        protected RunResult BadParameterError(string output) => new(this, Input, output, RunResultStatus.ArgumentError);
        protected RunResult ExceptionError(string output) => new(this, Input, output, RunResultStatus.ExceptionThrown);
        protected RunResult ContinueWith(string rawInput) => new(this, Input, _ouput.ToString(), RunResultStatus.ExceptionThrown, rawInput);
        #endregion

        #region Write helpers
        public void Write(string output, ConsoleColor? color = null) => _console.Write(GetType().Name, output, color);
        public void WriteLine(string output) => _console.WriteLine(GetType().Name, output, null);
        /// <summary>
        /// Could be use when passing the method to shell execute and you need to get back what was written and you do not want that in a logfile (a secret for example)
        /// </summary>
        /// <param name="output"></param>
        public void ReadLine(string output) => LastReadLine = output;
        public void WriteCodeExample(string commandName, string text) => _console.WriteCodeExample(GetType().Name, commandName, text);
        public void WriteHeadLine(string output) => _console.WriteHeaderLine(GetType().Name, output);
        public void WriteSuccess(string output) => _console.WriteSuccess(GetType().Name, output);
        public void WriteSuccessLine(string output) => _console.WriteSuccessLine(GetType().Name, output);
        public void WriteFailure(string output) => _console.Write(GetType().Name, output, ConsoleColor.DarkRed);
        public void WriteFailureLine(string output) => _console.WriteLine(GetType().Name, output, ConsoleColor.DarkRed);
        public void WriteWarning(string output) => _console.WriteWarning(GetType().Name, output);
        public void WriteError(string output) => _console.WriteError(GetType().Name, output);
        public void WriteCritical(string output) => _console.WriteCritical(GetType().Name, output);
        protected void OverwritePreviousLine(string output)
        {
            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1);
            var padRight = Console.BufferWidth - output.Length;
            AppendToOutput = false;
            WriteLine(output.PadRight(padRight > 0 ? padRight : 0));
            AppendToOutput = true;
        }
        #endregion
        protected void WriteProcessLog(string processTag, string processDescription) => IPowerCommandServices.DefaultInstance?.Logger.LogInformation($"#{processTag}# {processDescription}");
    }
}