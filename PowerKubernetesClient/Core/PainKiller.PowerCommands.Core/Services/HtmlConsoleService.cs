using Microsoft.Extensions.Logging;

namespace PainKiller.PowerCommands.Core.Services
{
    public class HtmlConsoleService : IConsoleService
    {
        private readonly string _colorStyleStandard = "style=\"color:white \"";
        private readonly string _colorStyleHeader = "style=\"color:cornflowerblue \"";
        private readonly string _colorStyleSuccess = "style=\"color:chartreuse \"";
        private readonly string _colorStyleFailure = "style=\"color:crimson \"";
        private readonly string _colorStyleCritical = "style=\"color:red \"";
        private readonly string _colorStyleWarning = "style=\"color:gold \"";

        private bool _disableLog;
        private static readonly Lazy<IConsoleService> Lazy = new(() => new HtmlConsoleService());
        public static IConsoleService Service => Lazy.Value;
        public event OnWrite? WriteToOutput;
        public void DisableLog()
        {
            WriteWarning(nameof(ConsoleService), "Log from ConsoleService is disabled");
            _disableLog = true;
        }
        public void EnableLog()
        {
            _disableLog = false;
            WriteToLog(nameof(ConsoleService), "Log from ConsoleService is enabled");
        }
        public void WriteObjectDescription(string scope, string name, string description, bool writeLog = true)
        {
            if (writeLog) WriteToLog(scope, $"{name} {description}");
            OnWriteToOutput($"<p><span {_colorStyleHeader}>{name}:</span><span {_colorStyleStandard}>{description}</span></p>\n");
        }
        public void Write(string scope, string text, ConsoleColor? color = null, bool writeLog = true)
        {
            if (writeLog) WriteToLog(scope, $"{text}");
            OnWriteToOutput($"<span {_colorStyleStandard}>{text}</span>");
        }
        public void WriteSuccess(string scope, string text, bool writeLog = true)
        {
            if (writeLog) WriteToLog(scope, $"{text}");
            OnWriteToOutput($"<span {_colorStyleSuccess}>{text}</span>");
        }
        public void WriteSuccessLine(string scope, string text, bool writeLog = true)
        {
            if (writeLog) WriteToLog(scope, $"{text}");
            OnWriteToOutput($"<p {_colorStyleSuccess}>{text}</p>\n");
        }
        public void WriteLine(string scope, string text, ConsoleColor? color = null, bool writeLog = true)
        {
            if (writeLog) WriteToLog(scope, $"{text}");
            OnWriteToOutput($"<p {_colorStyleStandard}>{text}</p>\n");
        }

        public void WriteCodeExample(string scope, string commandName, string text, ConsoleColor? color = null, bool writeLog = true)
        {
            if (writeLog) WriteToLog(scope, $" {commandName} {text}");
            OnWriteToOutput($"<p{_colorStyleHeader}>{commandName} <span></span> <span {_colorStyleStandard}>{text}</span></p>\n");
        }

        public void WriteHeaderLine(string scope, string text, ConsoleColor color = ConsoleColor.DarkCyan, bool writeLog = true)
        {
            if (writeLog) WriteToLog(scope, $"{text}");
            OnWriteToOutput($"<p {_colorStyleHeader}>{text}</p>\n");
        }
        public void WriteWarning(string scope, string text)
        {
            WriteToLog(scope, $"{text}", LogLevel.Warning);
            OnWriteToOutput($"<p {_colorStyleWarning}>{text}</p>\n");
        }
        public void WriteError(string scope, string text)
        {
            WriteToLog(scope, $"{text}", LogLevel.Error);
            OnWriteToOutput($"<p {_colorStyleFailure}>{text}</p>\n");
        }
        public void WriteCritical(string scope, string text)
        {
            WriteToLog(scope, $"{text}", LogLevel.Critical);
            OnWriteToOutput($"<p {_colorStyleCritical}>{text}</p>\n");
        }
        private void WriteToLog(string scope, string message, LogLevel level = LogLevel.Information)
        {
            if (_disableLog && level is LogLevel.Information or LogLevel.Debug or LogLevel.Trace) return;
            var text = $"{scope} {message}";
            switch (level)
            {
                case LogLevel.Trace:
                    IPowerCommandServices.DefaultInstance?.Logger.LogTrace(text);
                    break;
                case LogLevel.Information:
                    IPowerCommandServices.DefaultInstance?.Logger.LogInformation(text);
                    break;
                case LogLevel.Warning:
                    IPowerCommandServices.DefaultInstance?.Logger.LogWarning(text);
                    break;
                case LogLevel.Error:
                    IPowerCommandServices.DefaultInstance?.Logger.LogError(text);
                    break;
                case LogLevel.Critical:
                    IPowerCommandServices.DefaultInstance?.Logger.LogCritical(text);
                    break;
                case LogLevel.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }
        }
        protected virtual void OnWriteToOutput(string output) => WriteToOutput?.Invoke(output);
    }
}