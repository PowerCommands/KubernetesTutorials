using System.Reflection;

namespace PainKiller.PowerCommands.Core.Commands
{
    [PowerCommandTest(tests: " ")]
    [PowerCommandDesign(description: "Shows current version for the Core components.",
                 disableProxyOutput: true)]
    public class VersionCommand : CommandBase<CommandsConfiguration>
    {
        public VersionCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

        public override RunResult Run()
        {
            WriteLine($"{nameof(Core)}: {ReflectionService.Service.GetVersion(Assembly.Load($"PainKiller.PowerCommands.Core"))}");
            WriteLine($"{nameof(PowerCommands.Configuration)}: {ReflectionService.Service.GetVersion(Assembly.Load($"PainKiller.PowerCommands.Configuration"))}");
            WriteLine($"{nameof(ReadLine)}: {ReflectionService.Service.GetVersion(Assembly.Load($"PainKiller.PowerCommands.ReadLine"))}");
            WriteLine($"{nameof(Security)}: {ReflectionService.Service.GetVersion(Assembly.Load($"PainKiller.PowerCommands.Security"))}");
            WriteLine($"{nameof(Shared)}: {ReflectionService.Service.GetVersion(Assembly.Load($"PainKiller.PowerCommands.Shared"))}");
            return Ok();
        }
    }
}