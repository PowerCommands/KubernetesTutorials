namespace PainKiller.PowerCommands.Core.Commands;

[PowerCommandTest(        tests: " ")]
[PowerCommandDesign(description: "Change or view the current working directory",
             disableProxyOutput: true,
                        example: "//View current working directory|cd|//Traverse down one directory|//Change working directory|cd ..|cd \"C:\\ProgramData\"")]
public class CdCommand : CommandBase<CommandsConfiguration>
{
    public static string WorkingDirectory = AppContext.BaseDirectory;
    public CdCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }
    public override RunResult Run()
    {
        var path = WorkingDirectory;
        if (!string.IsNullOrEmpty(Input.Path))
        {
            path = Input.Path;
        }
        else if (Input.SingleArgument == "..")
        {
            var skipLast = path.EndsWith("\\") ? 2 : 1;
            var paths = WorkingDirectory.Split(Path.DirectorySeparatorChar).SkipLast(skipLast);
            path = string.Join(Path.DirectorySeparatorChar, paths);
        }
        else 
        {
            var dir = string.IsNullOrEmpty(Input.SingleArgument) ? Input.SingleQuote : Input.SingleArgument;
            var paths = WorkingDirectory.Split(Path.DirectorySeparatorChar).ToList();
            if(!string.IsNullOrEmpty(dir)) paths.Add(dir);
            path = string.Join(Path.DirectorySeparatorChar, paths);
        }

        if (Directory.Exists(path)) WorkingDirectory = path;
        else WriteFailureLine($"[{path}] does not exist");
        ShowDirectories();
        return Ok();
    }
    public static void ShowDirectories(string directory = "")
    {
        var dirInfo = new DirectoryInfo(string.IsNullOrEmpty(directory) ? WorkingDirectory : directory);
        Console.WriteLine(dirInfo.FullName);
        foreach (var directoryInfo in dirInfo.GetDirectories()) Console.WriteLine($"{directoryInfo.CreationTime}\t<DIR>\t{directoryInfo.Name}");
        foreach (var fileInfo in dirInfo.GetFiles()) Console.WriteLine($"{fileInfo.CreationTime}\t     \t{fileInfo.Name}");
    }
}