namespace PainKiller.PowerCommands.Configuration.DomainObjects;

public class ArtifactSourcePaths
{
    public string ExampleProjectFile { get; set; } = "PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.{name}Commands\\PainKiller.PowerCommands.MyExampleCommands.csproj";
    public string SolutionFileName { get; set; } = "PowerCommands2022\\src\\PainKiller.PowerCommands\\PowerCommands.{name}.sln";
    public string CommandsProject { get; set; } = "PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.MyExampleCommands";
    public string RenamedCommandsProject { get; set; } = "PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.{name}Commands";
    public string BootstrapProject { get; set; } = "PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.Bootstrap";
    public string ConsoleProject { get; set; } = "PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.PowerCommandsConsole";
    public string ThirdParty { get; set; } = "PowerCommands2022\\src\\PainKiller.PowerCommands\\Third party components";
    public string Template { get; set; } = "{_name}\\PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.MyExampleCommands\\Commands\\Templates";
    public string Backup { get; set; } = "\\download\\{_name}";
    public string Core { get; set; } = "PowerCommands2022\\src\\PainKiller.PowerCommands\\Core";
}