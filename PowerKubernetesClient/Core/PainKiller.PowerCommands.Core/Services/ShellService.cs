using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace PainKiller.PowerCommands.Core.Services
{
    public class ShellService : IShellService
    {
        private const int ImmediateReturn = 1000;
        private const int InfiniteWait = -1;
        private readonly ILogger _logger;

        private ShellService(ILogger logger) => _logger = logger;
        private static readonly Lazy<IShellService> Lazy = new(() => new ShellService(IPowerCommandServices.DefaultInstance!.Logger));
        public static IShellService Service => Lazy.Value;
        public void OpenDirectory(string directory)
        {
            var path = ReplaceCmdArguments(directory);
            _logger.LogInformation($"{nameof(ShellService)} {nameof(OpenDirectory)} {path}");
            if (!Directory.Exists(path)) return;
            Process.Start(new ProcessStartInfo { FileName = path, UseShellExecute = true, Verb = "open" });
        }
        public void OpenWithDefaultProgram(string uri)
        {
            _logger.LogInformation($"{nameof(ShellService)} {nameof(OpenDirectory)} {uri}");
            Process.Start(new ProcessStartInfo { FileName = uri, UseShellExecute = true, Verb = "open" });
        }
        public void Execute(string programName, string arguments, string workingDirectory, Action<string> writeFunction, string fileExtension = "exe", bool waitForExit = false, bool useShellExecute = false, bool disableOutputLogging = false)
        {
            var path = ReplaceCmdArguments(programName);
            var workingDirPath = ReplaceCmdArguments(workingDirectory);
            _logger.LogInformation($"{nameof(ShellService)} runs Execute with paramaters {path} {arguments} {workingDirPath} {fileExtension} {waitForExit}");
            var extension = string.IsNullOrEmpty(fileExtension) ? "" : $".{fileExtension}";
            var startInfo = new ProcessStartInfo
            {
                UseShellExecute = useShellExecute,
                RedirectStandardOutput = !useShellExecute,
                FileName = $"{path}{extension}",
                Arguments = arguments,
                WorkingDirectory = workingDirPath
            };

            var processAdd = Process.Start(startInfo);
            if (waitForExit)
            {
                var outputAdd = processAdd!.StandardOutput.ReadToEnd();
                writeFunction(outputAdd);
                if (!disableOutputLogging) _logger.LogInformation($"{nameof(ShellService)} output: {outputAdd}");
            }
            processAdd!.WaitForExit(waitForExit ? InfiniteWait : ImmediateReturn);
        }
        public void Execute(string programName, string arguments, string workingDirectory, string fileExtension = "exe", bool waitForExit = false, bool useShellExecute = false, bool disableOutputLogging = false)
        {
            var path = ReplaceCmdArguments(programName);
            var workingDirPath = ReplaceCmdArguments(workingDirectory);
            _logger.LogInformation($"{nameof(ShellService)} runs Execute with paramaters {path} {arguments} {workingDirPath} {fileExtension} {waitForExit}");
            var extension = string.IsNullOrEmpty(fileExtension) ? "" : $".{fileExtension}";
            var startInfo = new ProcessStartInfo
            {
                UseShellExecute = useShellExecute,
                RedirectStandardOutput = true,
                FileName = $"{path}{extension}",
                Arguments = arguments,
                WorkingDirectory = workingDirPath
            };
            var processAdd = Process.Start(startInfo);
            if (waitForExit)
            {
                var outputAdd = processAdd!.StandardOutput.ReadToEnd();
                Console.WriteLine(outputAdd);
                if (!disableOutputLogging) _logger.LogInformation($"{nameof(ShellService)} output: {outputAdd}");
            }
            processAdd!.WaitForExit(waitForExit ? InfiniteWait : ImmediateReturn);
        }
        private string ReplaceCmdArguments(string input) => input.Replace("%USERNAME%", Environment.UserName, StringComparison.CurrentCultureIgnoreCase);
    }
}