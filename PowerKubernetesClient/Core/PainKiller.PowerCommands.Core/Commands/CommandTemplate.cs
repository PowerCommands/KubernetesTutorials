namespace PainKiller.PowerCommands.Core.Commands
{
    public class CommandTemplate : CommandBase<CommandsConfiguration>
    {
        public CommandTemplate(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

        public override RunResult Run()
        {
            return Ok();
        }
    }
}