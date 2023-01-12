using PainKiller.PowerCommands.Shared.Contracts;

namespace PainKiller.PowerCommands.Shared.DomainObjects.Core;
public class CommandLineInput : ICommandLineInput
{
    public string Raw { get; init; } = "";
    public string Identifier { get; init; } = "";
    public string[] Quotes { get; init; } = new List<string>().ToArray();
    public string[] Arguments { get; init; } = new List<string>().ToArray();
    public string[] Options { get; init; } = new List<string>().ToArray();
    public string SingleArgument => Arguments.Length > 0 ? Arguments[0] : "";
    public string SingleQuote => Quotes.Length > 0 ? Quotes[0].Replace("\"", "") : "";
    public string Path { get; init; } = "";
}