using PainKiller.PowerCommands.Shared.DomainObjects.Core;

namespace PainKiller.PowerCommands.Shared.Contracts;

public interface IPowerCommandsRuntime
{
    string[] CommandIDs { get; }
    RunResult ExecuteCommand(string rawInput);
    List<IConsoleCommand> Commands { get; }
    public static IPowerCommandsRuntime? DefaultInstance { get; protected set; }
}