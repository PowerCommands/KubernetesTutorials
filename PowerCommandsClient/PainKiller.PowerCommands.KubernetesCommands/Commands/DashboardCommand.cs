namespace PainKiller.PowerCommands.KubernetesCommands.Commands;

[PowerCommandDesign( description: "Start up the Kubernetes dashboard",
                         example: "dashboard")]
public class DashboardCommand : CommandBase<PowerCommandsConfiguration>
{
    public DashboardCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }
    public override RunResult Run()
    {
        ShellService.Service.Execute("kubectl", "proxy", "", WriteLine, "", useShellExecute: true);
        var userName = "admin-user";
        DisableLog();
        ShellService.Service.Execute("kubectl", $"-n kubernetes-dashboard create token {userName}", "", ReadLine, "", waitForExit: true);
        Console.WriteLine(LastReadLine);
        EnableLog();
        ShellService.Service.OpenWithDefaultProgram(Configuration.KubernetesDashboardUrl);
        return Ok();
    }
}