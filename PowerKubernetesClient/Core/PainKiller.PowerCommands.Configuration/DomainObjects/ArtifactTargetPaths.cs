namespace PainKiller.PowerCommands.Configuration.DomainObjects
{
    public class ArtifactTargetPaths
    {
        public string ExampleProjectFile { get; set; } = "PainKiller.PowerCommands.MyExampleCommands\\PainKiller.PowerCommands.MyExampleCommands.csproj";
        public string SolutionFileName { get; set; } = "PainKiller.PowerCommands\\PowerCommands.{name}.sln";
        public string CommandsProject { get; set; } = "PainKiller.PowerCommands.{name}Commands";
        public string BootstrapProject { get; set; } = "PainKiller.PowerCommands.Bootstrap";
        public string ConsoleProject { get; set; } = "PainKiller.PowerCommands.PowerCommandsConsole";
        public string ThirdParty { get; set; } = "Third party components";
        public string Template { get; set; } = "Commands";
        public string Backup { get; set; } = "\\backup\\{_name}";
        public string Core { get; set; } = "Core";
    }
}