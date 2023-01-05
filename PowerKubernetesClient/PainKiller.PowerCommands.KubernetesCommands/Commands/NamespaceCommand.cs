namespace PainKiller.PowerCommands.KubernetesCommands.Commands;

[PowerCommandDesign( description: "Change the namespace for the current context",
                       arguments: "!<namespace name>",
                         example: "namespace my-namespace-name")]
public class NamespaceCommand : CommandBase<PowerCommandsConfiguration>
{
    public NamespaceCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        var nSpaceName = Input.SingleArgument;
        WriteCodeExample("kubectl",$"config set-context --current --namespace={nSpaceName}");
        ShellService.Service.Execute("kubectl",$"config set-context --current --namespace={nSpaceName}","", WriteLine,"", waitForExit: true);
        return Ok();
    }
}