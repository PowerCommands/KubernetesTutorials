namespace PainKiller.PowerCommands.Configuration.DomainObjects;

public class ArtifactPathsConfiguration
{
    public string Name { get; set; } = "Name";
    public string Download { get; set; } = "PainKiller.PowerCommands.MyExampleCommands";
    public string Templates { get; set; } = "{appdata}|templates";
    public string[] ValidProjectFiles { get; set; } = {
        "PainKiller.PowerCommands.Bootstrap\\PainKiller.PowerCommands.Bootstrap.csproj",
        "PainKiller.PowerCommands.PowerCommandsConsole\\PainKiller.PowerCommands.PowerCommandsConsole.csproj",
        "Core",
        "Third party components",
        "Third party components\\PainKiller.SerilogExtensions\\PainKiller.SerilogExtensions.csproj",
        "Core\\PainKiller.PowerCommands.Configuration\\PainKiller.PowerCommands.Configuration.csproj",
        "Core\\PainKiller.PowerCommands.Core\\PainKiller.PowerCommands.Core.csproj",
        "Core\\PainKiller.PowerCommands.ReadLine\\PainKiller.PowerCommands.ReadLine.csproj",
        "Core\\PainKiller.PowerCommands.Security\\PainKiller.PowerCommands.Security.csproj",
        "Core\\PainKiller.PowerCommands.Shared\\PainKiller.PowerCommands.Shared.csproj",
        "PainKiller.PowerCommands.MyExampleCommands\\PainKiller.PowerCommands.MyExampleCommands.csproj"
    };

    public string[] Commands { get; set; } = { "Demo", "Config", "Dir", "Doc"};
    public string[] TemplateCommands { get; set; } = { "Default" };
    public string VsCode { get; set; } = "PowerCommands2022\\.vscode\\";
    public string CustomComponents { get; set; } = "PowerCommands2022\\src\\PainKiller.PowerCommands\\Custom Components\\";
    public string DocsDbFile { get; set; } = "PowerCommands2022\\src\\PainKiller.PowerCommands\\Core\\PainKiller.PowerCommands.Core\\DocsDB.data";
    public string DocsDbGithub { get; set; } = "https://raw.githubusercontent.com/PowerCommands/PowerCommands2022/main/src/PainKiller.PowerCommands/Core/PainKiller.PowerCommands.Core/DocsDB.data";
    public string GithubRoot { get; set; } = "https://raw.githubusercontent.com/PowerCommands/PowerCommands2022/main/src/PainKiller.PowerCommands";
    public ArtifactSourcePaths Source { get; set; } = new();
    public ArtifactTargetPaths Target { get; set; } = new();
}