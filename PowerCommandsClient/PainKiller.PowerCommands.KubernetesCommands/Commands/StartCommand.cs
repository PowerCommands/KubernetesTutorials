namespace PainKiller.PowerCommands.KubernetesCommands.Commands;

[PowerCommandTest(         tests: " ")]
[PowerCommandDesign( description: "Description of your command...",
                         example: "demo")]
public class StartCommand : CommandBase<PowerCommandsConfiguration>
{
    public StartCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        var fullFileName = Path.Combine(Configuration.PathToDockerDesktop, "Docker Desktop.exe");
        ShellService.Service.Execute(fullFileName, arguments: "", workingDirectory: "", WriteLine, fileExtension: "");
        return Ok();
    }
}