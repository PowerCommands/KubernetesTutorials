namespace PainKiller.PowerCommands.Core.Commands
{
    [PowerCommandTest(tests: " ")]
    [PowerCommandDesign(description: "Change or view the current working directory",
                            options: "bookmark",
                 disableProxyOutput: true,
                            example: "//View current working directory|cd|//Traverse down one directory|//Change working directory|cd ..|cd \"C:\\ProgramData\"|//Set bookmark as the working directory using name|cd --bookmark program|//Set bookmark as the working directory using index|cd --bookmark 0|//Set first existing bookmark (if any) as working directory|cd --bookmark")]
    public class CdCommand : CommandBase<CommandsConfiguration>, IWorkingDirectoryChangesListener
    {
        public static string WorkingDirectory = AppContext.BaseDirectory;
        public static Action<string[], string[]>? WorkingDirectoryChanged;
        public CdCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }
        public override RunResult Run()
        {
            var path = WorkingDirectory;
            if (Input.SingleArgument == "\\")
            {
                var directory = new DirectoryInfo(WorkingDirectory);
                WorkingDirectory = directory.Root.FullName;
                ShowDirectories();
                return Ok();
            }

            var inputPath = string.IsNullOrEmpty(Input.Path) ? !string.IsNullOrEmpty(Input.SingleQuote) ? Path.Combine(path, Input.SingleQuote) : Path.Combine(path, string.Join(' ', Input.Arguments)) : Input.Path;

            var bookMarkIndex = Input.OptionToInt("bookmark", -1);
            var bookMark = GetOptionValue("bookmark");
            if (bookMarkIndex > -1)
            {
                if (bookMarkIndex > Configuration.Bookmark.Bookmarks.Count - 1)
                {
                    WriteError($"\nThere is no bookmark with index {bookMarkIndex} defined in {ConfigurationGlobals.MainConfigurationFile}\n");
                    WriteHeadLine("Defined bookmarks");
                    foreach (var b in Configuration.Bookmark.Bookmarks) WriteCodeExample($"{b.Index}", b.Name);
                    return Ok();
                }
                path = Configuration.Bookmark.Bookmarks[bookMarkIndex].Path;
            }
            else if (!string.IsNullOrEmpty(bookMark) && Configuration.Bookmark.Bookmarks.Any(b => b.Name.ToLower() == bookMark.ToLower()))
            {
                path = Configuration.Bookmark.Bookmarks.First(b => b.Name.ToLower() == bookMark.ToLower()).Path;
            }
            else if (HasOption("bookmark") && Configuration.Bookmark.Bookmarks.Count > 0)
            {
                path = Configuration.Bookmark.Bookmarks.First().Path;
            }
            else if (!string.IsNullOrEmpty(inputPath))
            {
                path = inputPath;
            }
            else if (Input.SingleArgument == "..")
            {
                var skipLast = path.EndsWith("\\") ? 2 : 1;
                var paths = WorkingDirectory.Split(Path.DirectorySeparatorChar).SkipLast(skipLast);
                path = string.Join(Path.DirectorySeparatorChar, paths);
            }
            else
            {
                var dir = string.IsNullOrEmpty(Input.SingleArgument) ? Input.SingleQuote : Input.SingleArgument;
                var paths = WorkingDirectory.Split(Path.DirectorySeparatorChar).ToList();
                if (!string.IsNullOrEmpty(dir)) paths.Add(dir);
                path = string.Join(Path.DirectorySeparatorChar, paths);
            }

            if (Directory.Exists(path)) WorkingDirectory = path;
            else WriteFailureLine($"[{path}] does not exist");
            ShowDirectories();
            return Ok();
        }
        private void ShowDirectories()
        {
            var dirInfo = new DirectoryInfo(WorkingDirectory);
            Console.WriteLine(dirInfo.FullName);
            var fileSuggestions = new List<string>();
            var dirSuggestions = new List<string>();
            foreach (var directoryInfo in dirInfo.GetDirectories())
            {
                Console.WriteLine($"{directoryInfo.CreationTime}\t<DIR>\t{directoryInfo.Name}");
                dirSuggestions.Add(directoryInfo.Name);
            }
            foreach (var fileInfo in dirInfo.GetFiles())
            {
                Console.WriteLine($"{fileInfo.CreationTime}\t     \t{fileInfo.Name}");
                fileSuggestions.Add(fileInfo.Name);
            }
            SuggestionProviderManager.AppendContextBoundSuggestions(Identifier, dirSuggestions.ToArray());
            WorkingDirectoryChanged?.Invoke(fileSuggestions.ToArray(), dirSuggestions.ToArray());

        }
        public virtual void OnWorkingDirectoryChanged(string[] files, string[] directories)
        {
            var suggestions = new List<string>();
            suggestions.AddRange(directories);
            if (Identifier != "cd") suggestions.AddRange(files);
            SuggestionProviderManager.AppendContextBoundSuggestions(Identifier, suggestions.ToArray());
        }
        public virtual void InitializeWorkingDirectory()
        {
            var id = Identifier;
            var dirInfo = new DirectoryInfo(WorkingDirectory);
            var fileSuggestions = dirInfo.GetFiles().Select(d => d.Name).ToArray();
            var dirSuggestions = dirInfo.GetDirectories().Select(d => d.Name).ToArray();
            WorkingDirectoryChanged?.Invoke(fileSuggestions, dirSuggestions);
        }
    }
}