using PainKiller.PowerCommands.Shared.Attributes;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;

namespace PainKiller.PowerCommands.Shared.Contracts
{
    public interface IConsoleCommand
    {
        string Identifier { get; }
        bool InitializeAndValidateInput(ICommandLineInput input, PowerCommandDesignAttribute designAttribute);
        void RunCompleted();
        RunResult Run();
        Task<RunResult> RunAsync();
    }
}