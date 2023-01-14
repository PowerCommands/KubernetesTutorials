namespace PainKiller.PowerCommands.Core.Managers
{
    public class VisualStudioManager : IVisualStudioManager
    {
        private readonly string _name;
        private readonly string _path;
        private readonly string _srcCodeRootPath;
        private readonly Action<string> _logger;

        public bool DisplayAndWriteToLog = true;
        public VisualStudioManager(string name, string path, Action<string> logger)
        {
            _name = name;
            _path = path;
            _srcCodeRootPath = Path.Combine(ConfigurationGlobals.ApplicationDataFolder, "download", name);
            _logger = logger;
        }
        public void CreateRootDirectory(bool onlyRepoSrcCodeRootPath = false)
        {
            if (!onlyRepoSrcCodeRootPath)
            {
                var dirI = new DirectoryInfo(_path);
                Directory.CreateDirectory(dirI.FullName);
                _logger.Invoke($"Directory {dirI.Attributes} created");
            }

            Directory.CreateDirectory(_srcCodeRootPath);
            _logger.Invoke($"Directory {_srcCodeRootPath} created");
        }
        public void DeleteDownloadsDirectory()
        {
            if (!Directory.Exists(_srcCodeRootPath)) return;
            var gitDirectory = Path.Combine(_srcCodeRootPath, "PowerCommands2022\\.git\\objects\\pack");
            if (!Directory.Exists(gitDirectory)) return;
            var dirInfo = new DirectoryInfo(gitDirectory);
            foreach (var fileSystemInfo in dirInfo.GetFileSystemInfos())
                File.SetAttributes(fileSystemInfo.FullName, FileAttributes.Normal);
            foreach (var file in dirInfo.GetFiles())
            {
                File.Delete(file.FullName);
            }
            DeleteDir(_srcCodeRootPath);
        }
        public void CreateDownloadsDirectory()
        {
            if (Directory.Exists(_srcCodeRootPath)) return;
            Directory.CreateDirectory(_srcCodeRootPath);
        }
        public void CreateDirectory(string name)
        {
            var dirI = new DirectoryInfo(Path.Combine(_path, name));
            Directory.CreateDirectory(dirI.FullName);
            _logger.Invoke($"Directory {dirI.Attributes} created");
        }
        public void CloneRepo(string repo) => ShellService.Service.Execute("git", $"clone {repo}", _srcCodeRootPath, _logger, waitForExit: true);
        public void DeleteDir(string directory)
        {
            var dirPath = GetPath(directory);
            _logger($"Delete directory {dirPath}");
            if (Directory.Exists(dirPath)) Directory.Delete(dirPath, recursive: true);
        }
        public void DeleteFile(string fileName, bool repoFile)
        {
            var path = repoFile ? Path.Combine(_srcCodeRootPath, fileName) : Path.Combine(_path, fileName);
            _logger($"Delete file {path}");
            if (File.Exists(path)) File.Delete(path);
        }
        public void RenameDirectory(string directory, string name)
        {
            var oldDirName = directory.Split('\\').Last();
            var newDirName = directory.Replace($"\\{oldDirName}", $"\\{name}");

            var oldDirPath = GetPath(directory);
            var newDirPath = GetPath(newDirName);
            Directory.Move(oldDirPath, newDirPath);

            _logger.Invoke("");
            _logger.Invoke($"Directory moved from [{directory}]");
            _logger.Invoke($"to [{newDirPath}]");
            _logger.Invoke("");
        }
        public void MoveFile(string fileName, string toFileName)
        {
            var oldFilePath = GetPath(fileName);
            var newFilePath = GetPath(toFileName);

            File.Move(oldFilePath, newFilePath);
            _logger.Invoke("");
            _logger.Invoke($"File moved from [{oldFilePath}]");
            _logger.Invoke($"to [{newFilePath}]");
            _logger.Invoke("");
        }
        public void MoveDirectory(string dirctoryName, string toDirctoryName)
        {
            var oldFilePath = GetPath(dirctoryName);
            var newFilePath = GetPath(toDirctoryName);

            Directory.Move(oldFilePath, newFilePath);
            _logger.Invoke("");
            _logger.Invoke($"Directory moved from [{oldFilePath}]");
            _logger.Invoke($"to [{newFilePath}]");
            _logger.Invoke("");
        }
        public string BackupDirectory(string dirctoryName)
        {
            var backupRoot = _srcCodeRootPath.Replace($"\\download\\{_name}", $"\\backup\\{_name}");
            if (!Directory.Exists(backupRoot)) Directory.CreateDirectory(backupRoot);
            var fullPathSource = Path.Combine(_path, dirctoryName);
            var fullPathTarget = backupRoot;

            if (Directory.Exists(fullPathTarget))
            {
                Console.WriteLine("Backup folder already exists, please remove that first.");
                ShellService.Service.OpenDirectory(fullPathTarget);
            }

            CopyFolder(fullPathSource, fullPathTarget);

            _logger.Invoke("");
            _logger.Invoke($"Directory [{fullPathSource}]");
            _logger.Invoke($"Backed up to [{fullPathTarget}]");
            _logger.Invoke("");
            return backupRoot;
        }
        public void WriteNewSolutionFile(string[] validProjectFiles)
        {
            var solutionFileSource = Path.Combine(_srcCodeRootPath, "PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.sln");
            var solutionFileNameTarget = Path.Combine(_srcCodeRootPath, $"PowerCommands2022\\src\\PainKiller.PowerCommands\\PowerCommands.{_name}.sln");

            ISolutionFileManager solutionFileManger = new SolutionFileManager(solutionFileSource);

            solutionFileManger.WriteValidProjectFiles(_name, validProjectFiles, solutionFileNameTarget);
            solutionFileManger.RemoveGlobalSectionNestedProjects(_name, solutionFileNameTarget);

            _logger.Invoke($"New solution file [{solutionFileNameTarget}] created");
        }
        public void ReplaceContentInFile(string fileName, string find, string replace)
        {
            var filePath = GetPath(fileName);
            var content = File.ReadAllText(filePath);
            content = content.Replace(find, replace);
            File.WriteAllText(filePath, content);
            _logger.Invoke($"Content replaced in file [{fileName}]");
        }
        public static string GetName()
        {
            var path = SolutionFileManager.GetLocalSolutionRoot();
            var solutionFile = Directory.GetFileSystemEntries(path, "*.sln").FirstOrDefault() ?? Directory.GetFileSystemEntries(AppContext.BaseDirectory, "*.exe").First().Replace(".exe", "");
            return solutionFile.Split('\\').Last().Replace(".sln", "");
        }
        public void MergeDocsDB() => GithubService.Service.MergeDocsDB();
        private string GetPath(string path) => path.StartsWith("PowerCommands2022\\") ? Path.Combine(_srcCodeRootPath, path) : Path.Combine(_path, path);
        private void CopyFolder(string sourceFolder, string destFolder) => IOService.CopyFolder(sourceFolder, destFolder);
    }
}