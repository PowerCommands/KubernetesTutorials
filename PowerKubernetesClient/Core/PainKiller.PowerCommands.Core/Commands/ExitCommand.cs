namespace PainKiller.PowerCommands.Core.Commands
{
    [PowerCommandDesign(description: "Exit command exits the program",
                           arguments: "<answer>(any answer starting with y will close the application)",
                          suggestions: "y",
                  disableProxyOutput: true,
                             example: "exit|exit y|exit Yes")]
    public class ExitCommand : CommandBase<CommandsConfiguration>
    {
        public ExitCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

        public override RunResult Run()
        {
            if (Input.Arguments.Length > 0 && Input.Arguments.First().ToLower().StartsWith("y")) return new RunResult(this, Input, output: "exit program", RunResultStatus.Quit);
            return DialogService.YesNoDialog("Do you wanna quit the program?") ? new RunResult(this, Input, output: "exit program", RunResultStatus.Quit) : new RunResult(this, Input, output: "No, do not exit the program", RunResultStatus.Ok);
        }
    }
}