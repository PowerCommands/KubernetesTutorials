namespace PainKiller.PowerCommands.Configuration.DomainObjects;

public class SecurityConfiguration
{
    public EncryptionConfiguration Encryption { get; set; } = new();
}