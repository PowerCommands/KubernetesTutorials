using PainKiller.PowerCommands.KubernetesCommands.DomainObjects;
using System.Text;
using PainKiller.PowerCommands.Core.Commands;

namespace PainKiller.PowerCommands.KubernetesCommands.Commands;

[PowerCommandDesign( description: "Publish your kubernetes application(s)",
                       arguments: "<directory name> where your manifests is located",
                         options: "!file|delete|!namespace",
                         example: "//Navigate to the directory where the directory (in this example named dashboard) to your manifest is|publish dashboard")]
public class PublishCommand : CdCommand
{
    public PublishCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        var name = Input.SingleArgument;
        var path = Path.Combine(CdCommand.WorkingDirectory, name);
        var nspace = GetOptionValue("namespace");
        var dirInfo = new DirectoryInfo(path);
        var publishFile = GetOptionValue("file");

        if (!string.IsNullOrEmpty(publishFile))
        {
            var singleFileName = Path.Combine(CdCommand.WorkingDirectory, publishFile);
            if (File.Exists(singleFileName))
            {
                if(publishFile.ToLower().EndsWith(".yaml")) ApplyYamlFile(dirInfo, nspace, singleFileName);
                if(publishFile.ToLower().EndsWith(".json")) HandleJsonFile(dirInfo, singleFileName);
                return Ok();
            }
            else
            {
                return BadParameterError($"File [{publishFile}] does not exist.");
            }
        }
        if (string.IsNullOrEmpty(name) && dirInfo.Exists)
        {
            WriteHeadLine("Kubernetes projects");
            foreach (var directoryInfo in dirInfo.GetDirectories())
            {
                WriteLine(directoryInfo.Name);
            }
            return Ok();
        }
        if(HasOption("delete")) UnPublish(path, dirInfo, nspace);
        else Publish(path, dirInfo, nspace);
        return Ok();
    }

    private void Publish(string path, DirectoryInfo dirInfo, string nspace)
    {
        var yamlFiles = Directory.GetFiles(path).OrderBy(f => f).ToList();
        foreach (var fileName in yamlFiles)
        {
            if(fileName.ToLower().EndsWith(".yaml")) ApplyYamlFile(dirInfo, nspace, fileName);
            if(fileName.ToLower().EndsWith(".json")) HandleJsonFile(dirInfo, fileName);
        }
        if (!string.IsNullOrEmpty(nspace)) ShellService.Service.Execute("kubectl",$"config set-context --current --namespace={nspace}","", WriteLine,"", waitForExit: true);
        ShellService.Service.Execute("kubectl", "get pods", dirInfo.FullName, WriteLine, "", waitForExit: true);
        ShellService.Service.Execute("kubectl", "get services", dirInfo.FullName, WriteLine, "", waitForExit: true);
        ShellService.Service.Execute("kubectl", "get deployments", dirInfo.FullName, WriteLine, "", waitForExit: true);
    }

    private void UnPublish(string path, DirectoryInfo dirInfo, string nSpace)
    {
        if(!string.IsNullOrEmpty(nSpace)) ShellService.Service.Execute("kubectl",$"config set-context --current --namespace={nSpace}","", WriteLine,"", waitForExit: true);
        var yamlFiles = Directory.GetFiles(path, "*.yaml").OrderByDescending(f => f).ToList();
        foreach (var fileName in yamlFiles)
        {
            var fileInfo = new FileInfo(fileName);
            ShellService.Service.Execute("kubectl",$"delete -f {fileInfo.FullName}",dirInfo.FullName, WriteLine,"", waitForExit: true);
            WriteSuccessLine($"{fileInfo.Name} deleted OK");
        }
    }

    private void HandleJsonFile(DirectoryInfo dirInfo,string fileName)
    {
        var fileInfo = new FileInfo(fileName);
        var processMetadata = StorageService<ProcessMetadata>.Service.GetObject(fileName);
        if (processMetadata.WaitSec > 0)
        {
            for (int i = 0; i < processMetadata.WaitSec; i++)
            {
                OverwritePreviousLine($"Waiting a {processMetadata.WaitSec-i} seconds, before executing [{processMetadata.Description}] ...");
                Thread.Sleep(1000);
            }
        }
        WriteLine(processMetadata.Description);
        WriteSuccessLine($"{fileInfo.Name} executed OK");
        if (string.IsNullOrEmpty(processMetadata.Url))
        {
            var applicationName = processMetadata.Name.Replace(".exe", "").Replace(".bat", "").Replace(".cmd", "");
            if (processMetadata.UseReadline)
            {
                ShellService.Service.Execute(applicationName, processMetadata.Args, WorkingDirectory, ReadLine, "", waitForExit: processMetadata.WaitForExit, useShellExecute: processMetadata.UseShellExecute, disableOutputLogging: processMetadata.DisableOutputLogging);
                var token = processMetadata.Base64Decode ?  Encoding.UTF8.GetString(Convert.FromBase64String(LastReadLine)) : LastReadLine;
                Console.WriteLine(token);
            }
            else ShellService.Service.Execute(applicationName, processMetadata.Args, WorkingDirectory, WriteLine, "", waitForExit: processMetadata.WaitForExit, useShellExecute: processMetadata.UseShellExecute, disableOutputLogging: processMetadata.DisableOutputLogging);
        }
        else
        {
            var url = processMetadata.Url.Replace(".exe", "").Replace(".bat", "").Replace(".cmd", "");
            ShellService.Service.OpenWithDefaultProgram(url);
        }
    }
    private void ApplyYamlFile(DirectoryInfo dirInfo, string nspace, string fileName)
    {
        var nmnSpace = nspace;
        if (!string.IsNullOrEmpty(nmnSpace) && !fileName.ToLower().Contains("namespace")) nmnSpace = $"-n {nspace}";
        else nmnSpace = "";
        var fileInfo = new FileInfo(fileName);
        ShellService.Service.Execute("kubectl", $"apply {nmnSpace} -f {fileInfo.FullName}", WorkingDirectory, WriteLine, "", waitForExit: true);
        WriteSuccessLine($"{fileInfo.Name} applied OK");
    }
}