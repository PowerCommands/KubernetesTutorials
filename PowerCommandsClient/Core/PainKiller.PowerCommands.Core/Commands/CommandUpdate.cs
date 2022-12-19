namespace PainKiller.PowerCommands.Core.Commands;
public class CommandUpdate : PowerCommandCommand
{
    private readonly ArtifactPathsConfiguration _artifact;
    private string _path = "";
    public CommandUpdate(string identifier, CommandsConfiguration configuration, ArtifactPathsConfiguration artifact, ICommandLineInput input) : base(identifier, configuration)
    {
        _artifact = artifact;
        Input = input;
    }
    public override RunResult Run()
    {
        var template = Input.HasOption("template");
        if (template)
        {
            _path = Path.Combine(AppContext.BaseDirectory, "output");
            UpdateTemplates(new VisualStudioManager(VisualStudioManager.GetName(), _path, WriteLine), cloneRepo: true);
            return Ok();
        }
        _path = SolutionFileManager.GetLocalSolutionRoot();
        var solutionFile = Directory.GetFileSystemEntries(_path, "*.sln").FirstOrDefault();
        var existingName = VisualStudioManager.GetName();
        var backup = Input.HasOption("backup");

        IVisualStudioManager vsm = new VisualStudioManager(existingName, _path, WriteLine);

        if (solutionFile == null)
        {
            WriteLine("When running in application scope (outside VS Env) only the Documentation file will be updated...");
            vsm.MergeDocsDB();
            return Ok();
        }

        Console.WriteLine("Update will delete and replace everything in the [Core] and [Third party components] folder");
        if (backup) Console.WriteLine($"A backup will be saved in folder [{Path.Combine(_path)}]", "Backup. /nEarlier backups needs to be removed");
        Console.WriteLine("");
        Console.WriteLine("Do you want to continue with the update? y/n");
        var response = Console.ReadLine();
        if ($"{response?.Trim()}" != "y") return Ok();

        vsm.DeleteDownloadsDirectory();
        vsm.CreateRootDirectory(onlyRepoSrcCodeRootPath: true);

        var backupDirectory = "";
        if (backup) backupDirectory = vsm.BackupDirectory(_artifact.Target.Core);

        vsm.CloneRepo(Configuration.Repository);
        WriteLine("Fetching repo from Github...");

        UpdateTemplates(vsm);

        vsm.DeleteDir(_artifact.VsCode);
        vsm.DeleteDir(_artifact.CustomComponents);

        vsm.DeleteDir(_artifact.Target.Core);
        vsm.MoveDirectory(_artifact.Source.Core, _artifact.Target.Core);

        vsm.DeleteDownloadsDirectory();

        WriteLine("Your PowerCommands Core component is now up to date with latest code from github!");
        WriteLine("if you started this from Visual Studio you probably need to restart Visual Studio to reload all dependencies");
        if (backup) WriteLine($"A backup of the Core projects has been stored here [{backupDirectory}]");

        ShellService.Service.OpenDirectory(backupDirectory);

        return Ok();
    }
}