namespace PainKiller.PowerCommands.KubernetesCommands.Configuration;

public class PowerCommandsConfiguration : CommandsConfiguration
{
    //Here is the placeholder for your custom configuration, you need to add the change to the PowerCommandsConfiguration.yaml file as well
    public string DefaultGitRepositoryPath { get; set; } = "C:\\repo";
    public string KubernetesDeploymentFilesRoot { get; set; } = "";
    public string KubernetesDashboardUrl { get; set; } = "";
    public string ArgoCdAdminUrl { get; set; } = "";
    public string MinIOAdminUrl { get; set; } = "";
    public string MinIOPortForward { get; set; } = "";
    public string PathToDockerDesktop { get; set; } = "";
}