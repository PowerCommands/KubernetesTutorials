namespace PainKiller.PowerCommands.KubernetesCommands.Commands;

[PowerCommandDesign( description: "Start the proxy for your MinIO instance",
                         example: "minio")]
public class MinioCommand : CommandBase<PowerCommandsConfiguration>
{
    public MinioCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        WriteCodeExample("kubectl",Configuration.MinIOPortForward);
        ShellService.Service.Execute("kubectl", Configuration.MinIOPortForward, "", WriteLine, "", useShellExecute: true);
        ShellService.Service.OpenWithDefaultProgram(Configuration.MinIOAdminUrl);
        return Ok();
    }
}