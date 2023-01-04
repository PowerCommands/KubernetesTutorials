namespace PainKiller.PowerCommands.KubernetesCommands.Commands;

[PowerCommandDesign( description: "Publish your kubernetes application(s)",
                         options: "!name|!namespace",
                         example: "publish --name bootcamp")]
public class PublishCommand : CommandBase<PowerCommandsConfiguration>
{
    public PublishCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        var name = GetOptionValue("name");
        var path = Path.Combine(Configuration.KubernetesDeploymentFilesRoot, name);

        var nspace = GetOptionValue("namespace");
        var dirInfo = new DirectoryInfo(path);

        if (string.IsNullOrEmpty(name) && dirInfo.Exists)
        {
            WriteHeadLine("Kubernetes projects");
            foreach (var directoryInfo in dirInfo.GetDirectories())
            {
                WriteLine(directoryInfo.Name);
            }
            return Ok();
        }

        var fileNames = Directory.GetFiles(path, "*.yaml").OrderBy(f => f).ToList();
        foreach (var fileName in fileNames)
        {
            var nmnSpace = nspace;
            if (!string.IsNullOrEmpty(nmnSpace) && !fileName.ToLower().Contains("namespace")) nmnSpace = $"-n {nspace}";
            else nmnSpace = "";
            var fileInfo = new FileInfo(fileName);
            ShellService.Service.Execute("kubectl",$"apply {nmnSpace} -f {fileInfo.FullName}",dirInfo.FullName, WriteLine,"", waitForExit: true);
            WriteSuccessLine($"{fileInfo.Name} applied OK");
        }
        if (!string.IsNullOrEmpty(nspace)) nspace = $"-n {nspace}";
        ShellService.Service.Execute("kubectl", $"get services {nspace}", dirInfo.FullName, WriteLine, "", waitForExit: true);
        return Ok();
    }
}