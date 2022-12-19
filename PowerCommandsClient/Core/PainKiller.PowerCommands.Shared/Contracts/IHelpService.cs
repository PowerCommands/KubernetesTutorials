namespace PainKiller.PowerCommands.Shared.Contracts;

public interface IHelpService
{
    void ShowHelp(IConsoleCommand command, bool clearConsole = true);
}