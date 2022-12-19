namespace PainKiller.PowerCommands.KubernetesCommands.Configuration;

public class PowerCommandsConfiguration : CommandsConfiguration
{
    //Here is the placeholder for your custom configuration, you need to add the change to the PowerCommandsConfiguration.yaml file as well
    public string DefaultGitRepositoryPath { get; set; } = "C:\\repos";
}