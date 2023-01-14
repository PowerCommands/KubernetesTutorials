namespace PainKiller.PowerCommands.Shared.Contracts
{
    public interface IConsoleWriter
    {
        void WriteLine(string output);
        void WriteHeadLine(string output);
        void Write(string output, ConsoleColor? color = null);
        void WriteSuccess(string output);
        void WriteFailure(string output);
    }
}