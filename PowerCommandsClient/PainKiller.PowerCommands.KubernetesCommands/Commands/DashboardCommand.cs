namespace PainKiller.PowerCommands.KubernetesCommands.Commands;

[PowerCommandDesign( description: "Start up the Kubernetes dashboard",
                         example: "dashboard")]
public class DashboardCommand : CommandBase<PowerCommandsConfiguration>
{
    public DashboardCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }
    public override RunResult Run()
    {
        ShellService.Service.Execute("kubectl", "proxy", "", WriteLine, "", useShellExecute: true);
        ShellService.Service.OpenWithDefaultProgram(Configuration.KubernetesDashboardUrl);
        return Ok();
    }
}