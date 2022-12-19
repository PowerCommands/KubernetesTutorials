namespace PainKiller.PowerCommands.Shared.DomainObjects.Configuration;

public class BaseComponentConfiguration
{
    public string Component { get; init; } = "";
    public string Checksum { get; set; } = "";
    public string Name { get; init; } = "";
}