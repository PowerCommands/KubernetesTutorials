namespace PainKiller.PowerCommands.KubernetesCommands.Commands;

[PowerCommandTest(         tests: " ")]
[PowerCommandDesign( description: "Description of your command...",
                         example: "demo")]
public class KCommand : CommandBase<PowerCommandsConfiguration>
{
    public KCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        ShellService.Service.Execute("kubectl", Input.Raw.Replace($"{Input.Identifier} ",""), "", WriteLine, "", waitForExit: true);
        return Ok();
    }
}