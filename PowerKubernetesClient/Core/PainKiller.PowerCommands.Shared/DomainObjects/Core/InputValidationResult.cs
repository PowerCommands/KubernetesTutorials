namespace PainKiller.PowerCommands.Shared.DomainObjects.Core;

public class InputValidationResult
{
    public bool HasValidationError { get; set; }
    public List<PowerOption> Options { get; set; } = new();
}