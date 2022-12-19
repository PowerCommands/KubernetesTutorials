namespace PainKiller.PowerCommands.Shared.DomainObjects.Configuration;

public record Metadata
{
    public string Name { get; init; } = nameof(Name);
    public string Description { get; init; } = nameof(Description);
}