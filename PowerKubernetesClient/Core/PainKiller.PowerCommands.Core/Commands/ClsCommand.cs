namespace PainKiller.PowerCommands.Core.Commands;

[PowerCommandTest(        tests: " ")]
[PowerCommandDesign(description: "Clears the console",
             disableProxyOutput: true)]
public class ClsCommand : CommandBase<CommandsConfiguration>
{
    public ClsCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }
    public override RunResult Run()
    {
        Console.Clear();
        return Ok();
    }
}