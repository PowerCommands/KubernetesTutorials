using System.Text.RegularExpressions;

namespace PainKiller.PowerCommands.Shared.Extensions
{
    public static class StringExtensions
    {
        public static string ToFirstLetterUpper(this string input)
        {
            if (input.Length < 2) return input.ToUpper();
            var firstLetter = input.Substring(0, 1).ToUpper();
            var retVal = $"{firstLetter}{input.Substring(1, input.Length - 1).ToLower()}";
            return retVal;
        }
        public static string ToOptionDescription(this string option)
        {
            var required = option.StartsWith('!') ? " (required)" : "";
            return string.IsNullOrEmpty(option) ? "" : $"--{option.Replace("!", "")}{required}";
        }
        public static string RemoveHtml(this string content) => Regex.Replace($"{content}", @"<[^>]*>", String.Empty);
        public static List<string> ExtractLinks(this string content) => new Regex("http(s)?://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?", RegexOptions.IgnoreCase).Matches(content).Select(m => m.Value).ToList();
    }
}