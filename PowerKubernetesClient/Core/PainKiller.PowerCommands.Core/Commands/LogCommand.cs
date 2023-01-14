namespace PainKiller.PowerCommands.Core.Commands
{
    [PowerCommandTest(tests: "--list|--view|--process git")]
    [PowerCommandDesign(description: "View and manage the log",
                              options: "view|archive|!process",
                   disableProxyOutput: true,
                              example: "//View a list with all the logfiles|log|//Archive the logs into a zip file.|log --archive|//View content of the current log|log --view|//Filter the log show only posts matching the provided process tag, this requires that you are using process tags when logging in your command(s).|log --process created")]
    public class LogCommand : CommandBase<CommandsConfiguration>
    {
        public LogCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

        public override RunResult Run()
        {
            if (Input.Options.Length == 0) List();
            if (Input.HasOption("archive")) Archive();
            if (Input.HasOption("view")) View();
            if (Input.HasOption("process")) ProcessLog($"{Input.GetOptionValue("process")}");

            return Ok();
        }
        private void List()
        {
            var dir = new DirectoryInfo(Path.Combine(AppContext.BaseDirectory, Configuration.Log.FilePath));
            foreach (var file in dir.GetFiles("*.log").OrderByDescending(f => f.LastWriteTime)) WriteLine($"{file.Name} {file.LastWriteTime}");
            Console.WriteLine();
            WriteHeadLine("To view current logfile type log view");
            WriteHeadLine("Example");
            ConsoleService.Service.WriteLine(nameof(LogCommand), "log --view");

            Console.WriteLine();
            WriteHeadLine("To archive the logs into a zip file type log --archive");
            WriteHeadLine("Example");
            ConsoleService.Service.WriteLine(nameof(LogCommand), "log --archive");
        }
        private void Archive() => WriteLine(Configuration.Log.ArchiveLogFiles());
        private void View()
        {
            foreach (var line in Configuration.Log.ToLines()) ConsoleService.Service.WriteLine(nameof(LogCommand), line);
        }
        private void ProcessLog(string processTag)
        {
            foreach (var line in Configuration.Log.GetProcessLog(processTag)) ConsoleService.Service.WriteLine(nameof(LogCommand), line);
        }
    }
}