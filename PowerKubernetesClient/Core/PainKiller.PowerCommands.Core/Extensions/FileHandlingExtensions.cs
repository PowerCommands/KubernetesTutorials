namespace PainKiller.PowerCommands.Core.Extensions
{
    public static class FileHandlingExtensions
    {
        public static int CopyFiles(this DirectoryInfo directoryInfo, string toPath)
        {
            var fileNames = directoryInfo.GetFiles();
            var fileCount = fileNames.Length;
            Directory.CreateDirectory(toPath);
            foreach (var fileInfo in fileNames) File.Copy(fileInfo.FullName, $"{toPath}\\{fileInfo.Name}");
            return fileCount;
        }
    }
}