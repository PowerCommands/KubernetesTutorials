namespace PainKiller.PowerCommands.Shared.Contracts
{
    public delegate void OnWrite(string output);
    public interface IConsoleService
    {
        event OnWrite WriteToOutput;
        void DisableLog();
        void EnableLog();
        void WriteObjectDescription(string scope, string name, string description, bool writeLog = true);
        void Write(string scope, string text, ConsoleColor? color = null, bool writeLog = true);
        void WriteLine(string scope, string text, ConsoleColor? color = null, bool writeLog = true);
        void WriteCodeExample(string scope, string commandName, string text, ConsoleColor? color = null, bool writeLog = true);
        void WriteHeaderLine(string scope, string text, ConsoleColor color = ConsoleColor.DarkCyan, bool writeLog = true);
        void WriteWarning(string scope, string text);
        void WriteError(string scope, string text);
        void WriteCritical(string scope, string text);
        void WriteSuccessLine(string scope, string text, bool writeLog = true);
        void WriteSuccess(string scope, string text, bool writeLog = true);
    }
}