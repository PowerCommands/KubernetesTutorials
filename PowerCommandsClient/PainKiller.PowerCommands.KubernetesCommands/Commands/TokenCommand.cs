namespace PainKiller.PowerCommands.KubernetesCommands.Commands;

[PowerCommandDesign( description: "Getting a Bearer Token",
                         options: "!username",
                         example: "token --username")]
public class TokenCommand : CommandBase<PowerCommandsConfiguration>
{
    public TokenCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        var userName = GetOptionValue("username");
        ShellService.Service.Execute("kubectl", $"-n kubernetes-dashboard create token {userName}", "", ReadLine, "", waitForExit: true, disableOutputLogging: true);
        Console.WriteLine(LastReadLine);
        return Ok();
    }
}