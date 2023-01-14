using System.Reflection;

namespace PainKiller.PowerCommands.Core.Services
{
    public class ReflectionService : IReflectionService
    {
        private ReflectionService() { }
        private static readonly Lazy<IReflectionService> Lazy = new(() => new ReflectionService());
        public static IReflectionService Service => Lazy.Value;
        public List<IConsoleCommand> GetCommands<TConfiguration>(BaseComponentConfiguration pluginInfo, TConfiguration configuration) where TConfiguration : CommandsConfiguration
        {
            var currentAssembly = Assembly.Load($"{pluginInfo.Component}".Replace(".dll", ""));
            return GetCommands(currentAssembly, configuration);
        }
        public List<IConsoleCommand> GetCommands<TConfiguration>(Assembly assembly, TConfiguration configuration) where TConfiguration : CommandsConfiguration
        {
            var retVal = new List<IConsoleCommand>();

            var types = assembly.GetTypes().Where(t => t.IsClass && t.Name.EndsWith("Command") && !t.IsAbstract).ToList();
            if (types.Count == 0) return retVal;

            foreach (var commandType in types)
            {
                var constructorInfo = commandType.GetConstructors()[0];
                var name = commandType.Name.ToLower();
                Object[] args = { name.Substring(0, name.Length - 7), (constructorInfo.GetParameters()[1].ParameterType == typeof(CommandsConfiguration) ? configuration as CommandsConfiguration : configuration) };
                var command = (IConsoleCommand)Activator.CreateInstance(commandType, args)!;
                var pcAttribute = command.GetPowerCommandAttribute();
                var suggestions = new List<string>();
                if (!string.IsNullOrEmpty(pcAttribute.Options)) suggestions.AddRange(pcAttribute.Options.Split('|').Select(f => $"--{f}"));
                suggestions.Add("--help");
                if (!string.IsNullOrEmpty(pcAttribute.Suggestions)) suggestions.AddRange(pcAttribute.Suggestions.Split('|').Select(f => $"{f}"));
                SuggestionProviderManager.AddContextBoundSuggestions(command.Identifier, suggestions.ToArray());
                AppendWorkingDirectoryListener(command);
                retVal.Add(command);
            }
            return retVal.OrderBy(c => c.Identifier).ToList();
        }
        private void AppendWorkingDirectoryListener(IConsoleCommand command)
        {
            var workingDirectoryListener = command.GetType().GetInterface(nameof(IWorkingDirectoryChangesListener));
            if (workingDirectoryListener != null)
            {
                // ReSharper disable once SuspiciousTypeConversion.Global
                var listener = (IWorkingDirectoryChangesListener)command;
                CdCommand.WorkingDirectoryChanged += listener.OnWorkingDirectoryChanged;
                listener.InitializeWorkingDirectory();
            }
        }
        public string GetVersion(Assembly assembly) => $"{assembly.GetName().Version!.Major}.{assembly.GetName().Version!.Minor}.{assembly.GetName().Version!.Build}.{assembly.GetName().Version!.Revision}";
    }
}