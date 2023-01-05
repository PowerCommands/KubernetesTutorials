namespace PainKiller.PowerCommands.KubernetesCommands.Commands;

[PowerCommandDesign(description: "Open a given directory or current working folder if argument is omitted, use option --app to open the AppData roaming directory",
                      arguments: "<directory name>",
                          options: "app",
                        example: "//Open the bin directory where this program resides|dir|//Open a path, you can use code completion with tab, just begin with a valid path first like C:|dir C:\\repos|//Open the AppData roaming directory|dir --app")]
public class DirCommand : CommandBase<CommandsConfiguration>
{
    public DirCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        var directory = string.IsNullOrEmpty(Input.Path) ? (Input.HasOption("app") ? ConfigurationGlobals.ApplicationDataFolder : AppContext.BaseDirectory) : Input.Path;
        if (!Directory.Exists(directory)) return BadParameterError($"Could not find directory \"{directory}\"");
        ShellService.Service.OpenDirectory(directory);
        WriteLine($"Open directory {directory}");
        return Ok();
    }
}