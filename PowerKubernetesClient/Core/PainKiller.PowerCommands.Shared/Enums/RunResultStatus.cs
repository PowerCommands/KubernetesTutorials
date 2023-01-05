namespace PainKiller.PowerCommands.Shared.Enums;

public enum RunResultStatus
{
    Ok,
    Quit,
    ExceptionThrown,
    ArgumentError,
    SyntaxError,
    RunExternalPowerCommand,
    InputValidationError,
    Initializing,
    Async,
    Help,
    Continue
}