using System.IO.Compression;
using System.Text;
using static System.DateTime;

namespace PainKiller.PowerCommands.Core.Extensions
{
    public static class LogExtensions
    {
        public static string FormatFileTimestamp(this string prefix) => $"{prefix}{Now.Year}{Now.Month}{Now.Day}{Now.Hour}{Now.Minute}";
        public static string PrefixFileTimestamp(this string fileName) => $"{Now.Year}{Now.Month}{Now.Day}{Now.Hour}{Now.Minute}{fileName}";

        public static string ArchiveLogFiles(this ILogComponentConfiguration configuration)
        {
            var retVal = new StringBuilder();

            var fileStamp = "archive".FormatFileTimestamp();
            var tempDirectory = $"{Path.GetTempPath()}\\{fileStamp}";
            var zipFileName = $"{configuration.FilePath}\\{fileStamp}.zip";

            var fileNames = Directory.GetFiles(configuration.FilePath, "*.log");

            Directory.CreateDirectory(tempDirectory);

            foreach (var fileName in fileNames)
            {
                var file = new FileInfo(fileName);
                File.Copy(fileName, $"{tempDirectory}\\{file.Name}");
                try { File.Delete(fileName); }
                catch (IOException) { retVal.AppendLine($"The file {fileName} is the current logfile and could not be deleted."); }
                retVal.AppendLine($"{fileName} moved to archive directory {tempDirectory}");
            }
            ZipFile.CreateFromDirectory(tempDirectory, zipFileName);
            Directory.Delete(tempDirectory, recursive: true);

            return retVal.ToString();
        }
        public static IEnumerable<string> ToLines(this ILogComponentConfiguration configuration)
        {
            var dir = new DirectoryInfo(Path.Combine(AppContext.BaseDirectory, configuration.FilePath));
            var currentFile = dir.GetFiles("*.log").OrderByDescending(f => f.LastWriteTime).First();
            var tempFileName = $"{Path.GetTempPath()}\\{currentFile.Name}".FormatFileTimestamp();
            File.Copy(currentFile.FullName, tempFileName);

            var lines = File.ReadAllLines(tempFileName).ToList();
            File.Delete(tempFileName);
            return lines;
        }

        public static IEnumerable<string> GetProcessLog(this ILogComponentConfiguration configuration, string processTag)
        {
            if (string.IsNullOrEmpty(processTag)) return new[] { "" };
            var dir = new DirectoryInfo(Path.Combine(AppContext.BaseDirectory, configuration.FilePath));
            var currentFile = dir.GetFiles("*.log").OrderByDescending(f => f.LastWriteTime).First();
            var tempFileName = $"{Path.GetTempPath()}\\{currentFile.Name}".FormatFileTimestamp();
            File.Copy(currentFile.FullName, tempFileName);

            var lines = File.ReadAllLines(tempFileName).Where(l => l.ToLower().Contains($"#{processTag.ToLower()}#")).ToList();
            File.Delete(tempFileName);
            return lines;
        }
    }
}