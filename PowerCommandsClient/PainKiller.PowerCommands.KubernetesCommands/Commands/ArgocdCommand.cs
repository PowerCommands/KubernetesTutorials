namespace PainKiller.PowerCommands.KubernetesCommands.Commands;

[PowerCommandTest(         tests: " ")]
[PowerCommandDesign( description: "Description of your command...",
                         example: "demo")]
public class ArgocdCommand : CommandBase<PowerCommandsConfiguration>
{
    public ArgocdCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        ShellService.Service.Execute("kubectl", "port-forward svc/argocd-server -n argocd 8080:443", "", WriteLine, "", useShellExecute: true);
        ShellService.Service.OpenWithDefaultProgram(Configuration.ArgoCdAdminUrl);
        return Ok();
    }
}