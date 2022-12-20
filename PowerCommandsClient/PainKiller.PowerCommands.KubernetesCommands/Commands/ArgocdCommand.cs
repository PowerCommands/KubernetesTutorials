using System.Text;

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
        ShellService.Service.Execute("kubectl", "-n argocd get secret argocd-initial-admin-secret -o jsonpath=\"{.data.password}\"", "", ReadLine, "", waitForExit: true);
        var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(_lastReadLine));
        
        Console.WriteLine($"argocd-initial-admin-secret:");
        Console.WriteLine(decoded);
        
        ShellService.Service.OpenWithDefaultProgram(Configuration.ArgoCdAdminUrl);
        return Ok();
    }
}