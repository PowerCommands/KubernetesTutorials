namespace PainKiller.PowerCommands.ReadLine.Extensions
{
    internal static class StringExtension
    {
        internal static string[] SortSuggestions(this string[] suggestions)
        {
            var options = suggestions.Where(s => s.StartsWith("--")).ToArray();
            var noOptions = suggestions.Where(s => !s.StartsWith("--")).ToArray();
            var retVal = new List<string>();
            retVal.AddRange(noOptions);
            retVal.AddRange(suggestions);
            return retVal.ToArray();
        }
    }
}