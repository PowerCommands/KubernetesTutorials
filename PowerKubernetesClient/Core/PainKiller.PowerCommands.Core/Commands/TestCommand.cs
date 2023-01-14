namespace PainKiller.PowerCommands.Core.Commands
{
    [PowerCommandDesign(description: "Run test on specific command or all commands, the must have a PowerCommandTest attribute declared on class level.",
                               options: "all|!command|trace",
                    disableProxyOutput: true,
                               example: "//Test a specific command|test --command commandName|//Test all commands (default) option could be omitted|test --all")]
    public class TestCommand : CommandBase<CommandsConfiguration>
    {
        public TestCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

        public override RunResult Run()
        {
            Console.Clear();
            var commands = IPowerCommandsRuntime.DefaultInstance?.Commands ?? new List<IConsoleCommand>();
            if (Input.HasOption("command"))
            {
                var command = commands.FirstOrDefault(c => c.Identifier == Input.GetOptionValue("command"));
                if (command != null)
                {
                    var items = GetTest(command);
                    if (!Input.HasOption("trace")) Console.Clear();

                    ConsoleTableService.RenderConsoleCommandTable(items.ToArray(), this);
                }
                else WriteError($"Could not find a command with identity [{Input.GetOptionValue("command")}]");
            }
            else if (Input.HasOption("all"))
            {
                var reports = GetTestReports().Where(r => !r.TestDisabled);
                if (!Input.HasOption("trace")) Console.Clear();
                ConsoleTableService.RenderConsoleCommandTable(reports.ToArray(), this);
            }
            return Ok();
        }
        private List<CommandTestItem> GetTest(IConsoleCommand command)
        {
            try
            {
                var attribute = command.GetAttribute<PowerCommandTestAttribute>();
                if (attribute.Disabled)
                {
                    WriteLine("Test for this command is not declared or disabled");
                    return new List<CommandTestItem> { new CommandTestItem { Disabled = true } };
                }
                ConsoleService.Service.DisableLog();
                var retVal = new List<CommandTestItem>();
                var runtime = IPowerCommandServices.DefaultInstance!.Runtime;
                foreach (var test in attribute.Tests.Split("|"))
                {
                    var result = runtime.ExecuteCommand($"{command.Identifier} {test.Replace("!", "")}");
                    var testItem = new CommandTestItem { ExpectedResult = !test.StartsWith("!"), Status = result.Status, Command = command.Identifier, Test = test };
                    retVal.Add(testItem);
                }
                ConsoleService.Service.EnableLog();
                return retVal;
            }
            catch
            {
                ConsoleService.Service.EnableLog();
                throw;
            }
        }
        private List<CommandTestReport> GetTestReports()
        {
            var retVal = new List<CommandTestReport>();
            var commands = IPowerCommandsRuntime.DefaultInstance?.Commands ?? new List<IConsoleCommand>();
            foreach (var command in commands)
            {
                var tests = GetTest(command);
                var report = new CommandTestReport { Command = command.Identifier, Failures = tests.Count(t => t.Success == "*NO*"), TestDisabled = tests.First().Disabled, Tests = tests.Count };
                retVal.Add(report);
            }
            return retVal;
        }
    }
}