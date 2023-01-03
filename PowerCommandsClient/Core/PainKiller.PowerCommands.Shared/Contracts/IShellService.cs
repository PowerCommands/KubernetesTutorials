using System.Diagnostics;

namespace PainKiller.PowerCommands.Shared.Contracts;

public interface IShellService
{
    void Execute(string programName, string arguments, string workingDirectory, string fileExtension = "exe", bool waitForExit = false, bool useShellExecute = false);
    void OpenDirectory(string directory);
    void OpenWithDefaultProgram(string uri);
    void Execute(string programName, string arguments, string workingDirectory, Action<string> writeFunction, string fileExtension = "exe", bool waitForExit = false, bool useShellExecute = false);
    Process GetProxyConsoleProcess(string programName, string arguments, string workingDirectory, string fileExtension = "exe");
}
