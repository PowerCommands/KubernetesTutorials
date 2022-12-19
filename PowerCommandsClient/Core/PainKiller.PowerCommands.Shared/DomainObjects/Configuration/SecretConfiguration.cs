namespace PainKiller.PowerCommands.Shared.DomainObjects.Configuration;

public class SecretConfiguration
{
    public List<SecretItemConfiguration> Secrets { get; set; } = new();
}