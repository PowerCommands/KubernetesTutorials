using PainKiller.PowerCommands.Core.Commands;

namespace PainKiller.PowerCommands.KubernetesCommands.Commands;

[PowerCommandDesign(description: "List the content of the working directory or this applications app directory, with the option to open the directory with the File explorer ",
    options: "open|app",
    example: "//List the content and open the current working directory|dir --open|//Open the AppData roaming directory|dir --app --open")]
public class DirCommand : CommandBase<CommandsConfiguration>
{
    public DirCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        var directory = Input.HasOption("app") ? ConfigurationGlobals.ApplicationDataFolder : CdCommand.WorkingDirectory;
        if (!Directory.Exists(directory)) return BadParameterError($"Could not find directory \"{directory}\"");
        if (HasOption("open")) ShellService.Service.OpenDirectory(directory);
        ShowDirectories();
        return Ok();
    }

    private void ShowDirectories()
    {
        var dirInfo = new DirectoryInfo(CdCommand.WorkingDirectory);
        Console.WriteLine(dirInfo.FullName);
        foreach (var directoryInfo in dirInfo.GetDirectories()) Console.WriteLine($"{directoryInfo.CreationTime}\t<DIR>\t{directoryInfo.Name}");
        foreach (var fileInfo in dirInfo.GetFiles()) Console.WriteLine($"{fileInfo.CreationTime}\t     \t{fileInfo.Name}");
    }
}