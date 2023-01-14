using System.Text.RegularExpressions;
using static System.String;

namespace PainKiller.PowerCommands.Core.Extensions
{
    public static class CommandLineInputInterpreterExtension
    {
        public static ICommandLineInput Interpret(this string raw, string defaultCommand = "commands")
        {
            if (IsNullOrEmpty(raw)) throw new ArgumentNullException(nameof(raw));
            raw = raw.Trim();
            var adjustedInput = raw.Trim();
            var options = adjustedInput.Split(' ').Where(r => !r.Contains('\"') && r.StartsWith("--")).ToArray();
            var quotes = Regex.Matches(adjustedInput, "\\\"(.*?)\\\"").ToStringArray().ToList();
            adjustedInput = quotes.Aggregate(adjustedInput, (current, quote) => current.Replace(quote, ""));
            var arguments = adjustedInput.Split(' ').Where(r => !r.Contains('\"') && !r.StartsWith("--")).Where(a => !IsNullOrEmpty(a)).ToList();
            var identifier = arguments.Count == 0 ? defaultCommand : $"{arguments[0].ToLower()}";
            if (arguments.Count > 0)
            {
                arguments.RemoveAt(0); //Remove identifier from arguments
                var attrib = GetAttributeOfCommand(identifier, defaultCommand);
                var requredOpptions = attrib.Options.Split('|').Where(o => o.StartsWith("!"));
                foreach (var option in requredOpptions)
                {
                    if (options.All(o => o != option)) continue;
                    var optionValue = GetOptionValue(options, option.Replace("!", ""), raw);
                    var argument = arguments.FirstOrDefault(a => a == optionValue);
                    if (!IsNullOrEmpty(argument)) arguments.Remove(argument);
                    var quote = quotes.FirstOrDefault(q => q == $"\"{optionValue}\"");
                    if (!IsNullOrEmpty(quote)) quotes.Remove(quote);
                }
            }
            var retVal = new CommandLineInput { Arguments = arguments.ToArray(), Identifier = identifier, Quotes = quotes.ToArray(), Options = options, Raw = raw, Path = arguments.ToArray().ToPath(quotes.ToArray(), raw) };
            return retVal;
        }
        public static string ToPath(this string[] inputArray, string[] quotes, string raw)
        {
            foreach (var quote in quotes) if (Path.IsPathFullyQualified(quote.Replace("\"", ""))) return quote.Replace("\"", "");

            if (inputArray.Length < 1) return "";
            var possiblePathArguments = raw.Split(' ').TakeWhile(argument => !argument.StartsWith("--") || argument.StartsWith("\"")).ToList();
            var path = Join(' ', possiblePathArguments.Skip(1));
            return !Path.IsPathFullyQualified(path) ? "" : path;
        }
        private static string[] ToStringArray(this MatchCollection matches)
        {
            var retVal = new List<string>();
            foreach (Match match in matches) retVal.Add(match.ToString());
            return retVal.ToArray();
        }
        public static int FirstArgumentToInt(this ICommandLineInput input) => (int.TryParse(input.SingleArgument, out var index) ? index : 0);
        public static int OptionToInt(this ICommandLineInput input, string optionName, int valueIfMissing = 0)
        {
            var optionValue = GetOptionValue(input, optionName);
            if (IsNullOrEmpty(optionValue)) return valueIfMissing;
            return (int.TryParse(optionValue, out var index) ? index : valueIfMissing);
        }
        public static string GetOptionValue(this ICommandLineInput input, string[] options)
        {
            foreach (var inputOption in input.Options) if (options.Any(f => $"--{f}" == inputOption)) return inputOption.Replace("--", "");
            return "";
        }
        public static string GetOptionValue(this ICommandLineInput input, string optionName) => GetOptionValue(input.Options, optionName, input.Raw);
        private static string GetOptionValue(string[] options, string optionName, string raw)
        {
            var quotes = Regex.Matches(raw, "\\\"(.*?)\\\"").ToStringArray();
            var option = options.FirstOrDefault(f => f == $"--{optionName.ToLower()}");
            if (IsNullOrEmpty(option)) return "";

            var firstQuotedOptionValueIfAny = FindFirstQuotedOptionValueIfAny(raw, quotes, optionName);
            if (!IsNullOrEmpty(firstQuotedOptionValueIfAny)) return firstQuotedOptionValueIfAny;

            short index = 0;
            var indexedInputs = raw.Split(' ').Select(r => new IndexedInput { Index = index += 1, Value = r }).ToList();
            var indexedInput = indexedInputs.FirstOrDefault(i => i.Value.ToLower() == option.ToLower());
            if (indexedInput == null) return "";
            var retVal = indexedInput.Index == indexedInputs.Count ? "" : indexedInputs.First(i => i.Index == indexedInput.Index + 1).Value.Replace("\"", "");
            return retVal.StartsWith("--") ? "" : retVal;   //A option could not have a option as it´s value
        }
        public static bool HasOption(this ICommandLineInput input, string optionName) => input.Options.Any(f => f == $"--{optionName}");
        public static bool NoOption(this ICommandLineInput input, string optionName) => !HasOption(input, optionName);
        public static bool MustHaveOneOfTheseOptionCheck(this ICommandLineInput input, string[] optionNames) => optionNames.Any(optionName => optionName.ToLower() == optionName);
        public static string FirstOptionWithValue(this ICommandLineInput input) => $"{input.Options.FirstOrDefault(o => !IsNullOrEmpty(o))}".Replace("!", "").Replace("--", "");
        public static void DoBadOptionCheck(this ICommandLineInput input, IConsoleCommand command)
        {
            var dokumentedOptions = command.GetPowerCommandAttribute().Options.Split('|');
            foreach (var option in input.Options) if (dokumentedOptions.All(f => $"--{f.ToLower().Replace("!", "")}" != option.ToLower())) ConsoleService.Service.WriteLine($"{input.Identifier}", $"Warning, option  [{option}] is not declared and probably unhandled in command [{command.Identifier}]", ConsoleColor.DarkYellow);
        }
        private static string FindFirstQuotedOptionValueIfAny(string raw, string[] quotes, string optionName)
        {
            if (quotes.Length == 0) return "";
            //First lets find out if the next parameter after the option is a surrounded by " characters
            var lastIndexOfOption = raw.LastIndexOf($"--{optionName}", StringComparison.Ordinal) + $"--{optionName}".Length;
            var quotesAfterOption = quotes.Where(q => raw.LastIndexOf(q, StringComparison.Ordinal) > lastIndexOfOption).Select(q => new { Index = raw.LastIndexOf(q, StringComparison.Ordinal), Quote = q }).ToList();
            var firstQuoteAfterOption = quotesAfterOption.FirstOrDefault(q => q.Index == quotesAfterOption.Min(arg => arg.Index));
            if (firstQuoteAfterOption == null) return "";
            var diff = firstQuoteAfterOption.Index - lastIndexOfOption;
            return diff > 1 ? "" : firstQuoteAfterOption.Quote.Replace("\"", "");
        }
        public static string GetOutputFilename(this IConsoleCommand command) => Path.Combine(ConfigurationGlobals.ApplicationDataFolder, $"proxy_{command.Identifier}.data");
        private static PowerCommandDesignAttribute GetAttributeOfCommand(string identifier, string defaultCommandIdentifier)
        {
            var command = IPowerCommandsRuntime.DefaultInstance!.Commands.FirstOrDefault(c => c.Identifier == identifier) ?? IPowerCommandsRuntime.DefaultInstance.Commands.FirstOrDefault(c => c.Identifier == defaultCommandIdentifier);
            if (command == null) return new PowerCommandDesignAttribute("no attribute found");
            return command.GetPowerCommandAttribute();
        }
    }
}