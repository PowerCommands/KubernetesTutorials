namespace PainKiller.PowerCommands.Shared.Contracts
{
    public interface ICommandLineInput
    {
        string Raw { get; init; }
        string Identifier { get; init; }
        string[] Quotes { get; init; }
        string[] Arguments { get; init; }
        string[] Options { get; init; }
        string SingleArgument { get; }
        string SingleQuote { get; }
        string Path { get; init; }
    }
}