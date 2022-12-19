using PainKiller.PowerCommands.ReadLine.Contracts;

namespace PainKiller.PowerCommands.ReadLine.Handlers;
public class AutoCompleteHandler : IAutoCompleteHandler
{
    private readonly IEnumerable<string> _suggestions;
    private readonly Func<string, string[]> _suggestionProvider;

    public AutoCompleteHandler(IEnumerable<string> suggestions, Func<string, string[]> suggestionProvider)
    {
        _suggestions = suggestions;
        _suggestionProvider = suggestionProvider;
    }

    public char[] Separators { get; set; } = { ' ', '\\' };

    public string[] GetSuggestions(string input, int index)
    {
        var providerSuggestions = _suggestionProvider.Invoke(input);
        if (providerSuggestions != null) return providerSuggestions;
        var filter = _suggestions.Where(s => s.StartsWith(input)).Select(f => f.Substring(0, f.Length)).ToArray();
        return filter;
    }
}