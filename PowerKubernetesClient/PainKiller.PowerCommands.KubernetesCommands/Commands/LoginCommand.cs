using PainKiller.PowerCommands.Core.Commands;

namespace PainKiller.PowerCommands.KubernetesCommands.Commands;

[PowerCommandDesign( description: "Start your login cmd prompt, command exist in your PowerCommandsConfiguration.yaml file.",
    example: "login")]
public class LoginCommand : CommandBase<PowerCommandsConfiguration>
{
    public LoginCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        ShellService.Service.Execute("kubectl", Configuration.LoginShellCommand, CdCommand.WorkingDirectory, ReadLine, "", useShellExecute: true, disableOutputLogging: true);
        return Ok();
    }
}