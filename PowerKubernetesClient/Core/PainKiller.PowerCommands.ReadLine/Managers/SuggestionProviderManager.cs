namespace PainKiller.PowerCommands.ReadLine.Managers;

public class SuggestionProviderManager
{
    private static readonly Dictionary<string, string[]> ContextBoundSuggestions = new();

    public Func<string, string[]> SuggestionProviderFunc;
    public SuggestionProviderManager() => SuggestionProviderFunc = GetSuggestions;
    public static void AddContextBoundSuggestions(string contextId, string[] suggestions)
    {
        var excludeComments = suggestions.Where(s => !s.StartsWith("--//")).Select(s => s.Replace("!","")).ToArray();
        if(!ContextBoundSuggestions.ContainsKey(contextId)) ContextBoundSuggestions.Add(contextId, excludeComments);
    }

    private static string[] GetSuggestions(string input)
    {
        try
        {
            var inputs = input.Split(" ").ToList();
            if (inputs.Count < 2) return null!;

            var contextId = inputs.First();
            inputs.RemoveAt(0);

            if (ContextBoundSuggestions.ContainsKey(contextId) && inputs.Last().StartsWith("-")) return ContextBoundSuggestions.Where(c => c.Key == contextId).Select(c => c.Value).First();

            var buildPath = new List<string>();
            var startOfPathNotFound = true;
            foreach (var inputFragment in inputs)
            {
                if (inputFragment.Contains(":\\")) startOfPathNotFound = false;
                if (!startOfPathNotFound) buildPath.Add(inputFragment);
            }
            var filePath = string.Join(" ", buildPath).Replace("\"", "");
            if (!Directory.Exists(filePath)) return null!;

            var directoryInfo = new DirectoryInfo(filePath);
            var filter = directoryInfo.GetDirectories().Select(f => f.Name.Substring(0, f.Name.Length)).ToList();
            var fileFilter = directoryInfo.GetFiles().Select(f => f.Name.Substring(0, f.Name.Length)).ToArray();
            filter.AddRange(fileFilter);
            var retVal = filter.ToArray();
            return retVal;
        }
        catch (Exception e)
        {
            return new[] { e.Message };
        }
    }
}