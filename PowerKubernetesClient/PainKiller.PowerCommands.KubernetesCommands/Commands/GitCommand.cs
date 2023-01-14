namespace PainKiller.PowerCommands.KubernetesCommands.Commands;

[PowerCommandDesign(description: "Example shows how to execute a external program, in this case git, commit and push your repository, path to repository is in the configuration file",
                arguments: "commit|push|status|log|branch",
                  options: "create|change|delete|merge|main|relative-path",
                   quotes: "\"<comment>\" defaults to \"refactoring\" if omitted, only used with commit.",
              suggestions: "status|commit|push|log|branch",
                  example: "//Add and commit|git commit \"Bugfix\"|//Performs a push to Git repo|git push|//Git status of the configured git repo|git status|//Show log|git log|//Create and change to branch|git branch --create my-branch|//Change branch|git branch --change my-branch|//Merge branch|git --merge my-branch|//Delete branch locally (and remote if you want)|git --delete my-branch|//Change to main branch|git branch main")]

public class GitCommand : CommandBase<PowerCommandsConfiguration>
{
    public GitCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }
    public override RunResult Run()
    {
        if (HasOption("relative-path"))
        {
            var relativePath = GetGitRelativePath();
            WriteCodeExample("Relative path", relativePath);
            return Ok();
        }
        switch (Input.SingleArgument)
        {
            case "commit":
                Commit(Input.SingleQuote);
                break;
            case "branch":
                if (HasOption("change")) RunSingleCommand("checkout", GetOptionValue("change"));
                if (HasOption("create")) Create(GetOptionValue("create"));
                if (HasOption("main")) RunSingleCommand("checkout", "main");
                if (HasOption("merge")) Merge(GetOptionValue("merge"));
                if (HasOption("delete")) Delete(GetOptionValue("delete"));
                break;
            case "merge":
                Merge(Input.SingleQuote);
                break;
            case "push":
            case "status":
            case "log":
                RunSingleCommand(Input.SingleArgument, $"{Input.SingleQuote}");
                break;
            default:
                return BadParameterError($"Parameter {Input.SingleArgument} not supported");
        }
        return Ok();
    }
    private void Commit(string comment)
    {
        if (string.IsNullOrEmpty(comment)) comment = "\"refactoring\"";
        RunSingleCommand("add .");
        ShellService.Service.Execute("git", $"commit -m \"{comment}\"", Configuration.DefaultGitRepositoryPath, WriteLine, waitForExit: true);
        WriteProcessLog("GIT", $"commit m \"{comment}\"");
    }
    private void RunSingleCommand(string command, string name = "")
    {
        WriteHeadLine($"Local repo path: {Configuration.DefaultGitRepositoryPath}\n");
        ShellService.Service.Execute("git", $"{command} {name}", Configuration.DefaultGitRepositoryPath, WriteLine, waitForExit: true);
        WriteProcessLog("GIT", $"{command} {name}");
    }
    private void Merge(string branchName)
    {
        RunSingleCommand("checkout", "main");
        ShellService.Service.Execute("git", $"merge \"{branchName}\"", Configuration.DefaultGitRepositoryPath, WriteLine, waitForExit: true);
        WriteProcessLog("GIT", $"merge \"{branchName}\"");
    }
    private void Delete(string branchName)
    {
        RunSingleCommand("checkout", "main");

        //Locally
        ShellService.Service.Execute("git", $"branch \"{branchName}\" -D", Configuration.DefaultGitRepositoryPath, WriteLine, waitForExit: true);
        WriteProcessLog("GIT", $"branch \"{branchName}\" -D");
        var deleteRemote = DialogService.YesNoDialog("Do you also want to delete the branch remote (on the server)?");
        if (!deleteRemote) return;
        //Remote (server)
        ShellService.Service.Execute("git", $"push origin --delete \"{branchName}\"", Configuration.DefaultGitRepositoryPath, WriteLine, waitForExit: true);
        WriteProcessLog("GIT", $"push origin --delete \"{branchName}\"");
    }
    private void Create(string branchName)
    {
        RunSingleCommand("branch", branchName);

        ShellService.Service.Execute("git", $"checkout \"{branchName}\"", Configuration.DefaultGitRepositoryPath, WriteLine, waitForExit: true);
        WriteProcessLog("GIT", $"checkout \"{branchName}\"");

        ShellService.Service.Execute("git", $"push --set-upstream origin \"{branchName}\"", Configuration.DefaultGitRepositoryPath, WriteLine, waitForExit: true);
        WriteProcessLog("GIT", $"push --set-upstream origin \"{branchName}\"");
    }

    private string GetGitRelativePath()
    {
        var path = AppContext.BaseDirectory;
        var relativePath = "";
        var gitFound = false;
        var maxRepeatCount = 15;
        var iterationCount = 0;
        while (gitFound == false)
        {
            iterationCount++;
            var skipLast = path.EndsWith("\\") ? 2 : 1;
            var paths = path.Split(Path.DirectorySeparatorChar).SkipLast(skipLast);
            path = string.Join(Path.DirectorySeparatorChar, paths);
            var directory = new DirectoryInfo(path);
            gitFound = directory.GetDirectories().Any(d => d.Name.StartsWith(".git"));
            if (!gitFound) relativePath += "..\\";
            if(iterationCount > maxRepeatCount) break;
        }

        return relativePath;
    }
}