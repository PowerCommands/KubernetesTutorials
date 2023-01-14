using PainKiller.PowerCommands.Configuration.DomainObjects;

namespace PainKiller.PowerCommands.Configuration.Extensions
{
    public static class ConfigurationExtension
    {
        public static string GetSafePathRegardlessHowApplicationStarted(this string fileName, string directory = "") => string.IsNullOrEmpty(directory) ? Path.Combine(AppContext.BaseDirectory, fileName) : Path.Combine(AppContext.BaseDirectory, directory, fileName);

        public static string GetPath(this ArtifactPathsConfiguration configuration, string config)
        {
            var paths = config.Split('|');
            if (string.IsNullOrEmpty(config)) return "";
            if (paths.Length == 1) return ReplaceTags(config, configuration.Name);
            if (paths.Length == 2) return Path.Combine(ReplaceTags(paths[0], configuration.Name), ReplaceTags(paths[1], configuration.Name));
            return Path.Combine(ReplaceTags(paths[0], configuration.Name), ReplaceTags(paths[1], configuration.Name), ReplaceTags(paths[2], configuration.Name));
        }
        private static string ReplaceTags(string raw, string name) => raw.Replace("{appdata}", AppContext.BaseDirectory).Replace("{name}", name);
    }
}