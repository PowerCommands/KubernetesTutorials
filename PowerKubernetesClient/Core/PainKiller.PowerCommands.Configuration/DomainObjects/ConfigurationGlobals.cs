namespace PainKiller.PowerCommands.Configuration.DomainObjects
{
    public static class ConfigurationGlobals
    {
        public const string Prompt = "pcm>";
        public const string MainConfigurationFile = "PowerCommandsConfiguration.yaml";
        public const string SecurityFileName = "security.yaml";
        public static readonly string ApplicationDataFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\{nameof(PowerCommands)}";
    }
}