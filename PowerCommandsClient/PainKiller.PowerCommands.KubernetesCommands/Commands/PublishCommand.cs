namespace PainKiller.PowerCommands.KubernetesCommands.Commands;

[PowerCommandDesign( description: "Publish your kubernetes application(s)",
                         options: "!name",
                         example: "publish --name bootcamp")]
public class PublishCommand : CommandBase<PowerCommandsConfiguration>
{
    public PublishCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        var name = GetOptionValue("name");
        var path = Path.Combine(Configuration.KubernetesDeploymentFilesRoot, name);
        var dirInfo = new DirectoryInfo(path);

        var fileNames = Directory.GetFiles(path, "*.yaml").OrderBy(f => f).ToList();
        foreach (var fileName in fileNames)
        {
            var fileInfo = new FileInfo(fileName);
            ShellService.Service.Execute("kubectl",$"apply -f {fileInfo.FullName}",dirInfo.FullName, WriteLine,"", waitForExit: true);
            WriteSuccessLine($"{fileInfo.Name} applied OK");
        }

        ShellService.Service.Execute("kubectl", "get services", dirInfo.FullName, WriteLine, "", waitForExit: true);
        return Ok();
    }
}