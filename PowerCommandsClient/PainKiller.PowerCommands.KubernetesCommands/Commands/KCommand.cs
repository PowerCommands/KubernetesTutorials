namespace PainKiller.PowerCommands.KubernetesCommands.Commands;

[PowerCommandTest(         tests: " ")]
[PowerCommandDesign( description: "Description of your command...",
                         example: "demo")]
public class KCommand : CommandBase<PowerCommandsConfiguration>
{
    public KCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        ShellService.Service.Execute("kubectl", string.Join(' ',Input.Arguments), "", WriteLine, "", waitForExit: true);
        return Ok();
    }
}