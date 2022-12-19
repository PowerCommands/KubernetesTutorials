namespace PainKiller.PowerCommands.Shared.DomainObjects.Configuration;
public class SecretItemConfiguration
{
    public SecretItemConfiguration() => Options.Add("target","User");
    public string Name { get; set; } = "command-name-password";
    public Dictionary<string, string> Options { get; set; } = new();
}