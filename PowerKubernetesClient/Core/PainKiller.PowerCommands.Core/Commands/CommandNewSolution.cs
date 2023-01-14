namespace PainKiller.PowerCommands.Core.Commands
{
    public class CommandNewSolution : PowerCommandCommand
    {
        private readonly ArtifactPathsConfiguration _artifact;
        private string _path = "";
        public CommandNewSolution(string identifier, CommandsConfiguration configuration, ArtifactPathsConfiguration artifact, ICommandLineInput input) : base(identifier, configuration)
        {
            _artifact = artifact;
            Input = input;
        }
        public override RunResult Run()
        {
            var name = Input.GetOptionValue("solution");
            var output = Input.GetOptionValue("output");

            _path = string.IsNullOrEmpty(output) ? Path.Combine(AppContext.BaseDirectory, "output", name) : Path.Combine(output, name);
            IVisualStudioManager vsm = new VisualStudioManager(name, _path, WriteLine);
            vsm.DeleteDownloadsDirectory();
            vsm.CreateRootDirectory();

            vsm.CloneRepo(Configuration.Repository);
            WriteLine("Fetching repo from Github...");

            UpdateTemplates(vsm, newProjectName: name);

            vsm.DeleteDir(_artifact.VsCode);
            vsm.DeleteDir(_artifact.CustomComponents);

            vsm.RenameDirectory(_artifact.Source.CommandsProject, _artifact.GetPath(_artifact.Target.CommandsProject));

            vsm.WriteNewSolutionFile(_artifact.ValidProjectFiles);

            vsm.ReplaceContentInFile($"{_artifact.Source.BootstrapProject}\\PainKiller.PowerCommands.Bootstrap.csproj", "MyExampleCommands", $"{name}Commands");
            vsm.ReplaceContentInFile($"{_artifact.Source.ConsoleProject}\\PainKiller.PowerCommands.PowerCommandsConsole.csproj", "<AssemblyName>pc</AssemblyName>", $"<AssemblyName>{name}</AssemblyName>");
            vsm.ReplaceContentInFile($"{_artifact.Source.BootstrapProject}\\PowerCommandsManager.cs", "MyExampleCommands", $"{name}Commands");
            vsm.ReplaceContentInFile($"{_artifact.Source.BootstrapProject}\\Startup.cs", "MyExampleCommands", $"{name}Commands");
            vsm.ReplaceContentInFile($"{_artifact.Source.ConsoleProject}\\Program.cs", "Power Commands 1.0", $"{name} Commands 1.0");
            vsm.ReplaceContentInFile($"{_artifact.GetPath(_artifact.Source.RenamedCommandsProject)}\\PowerCommandsConfiguration.yaml", "My Example Command", $"{name} Commands");
            vsm.ReplaceContentInFile($"{_artifact.GetPath(_artifact.Source.RenamedCommandsProject)}\\PowerCommandsConfiguration.yaml", "MyExampleCommands", $"{name}Commands");
            vsm.ReplaceContentInFile($"{_artifact.GetPath(_artifact.Source.RenamedCommandsProject)}\\ArtifactPathsConfiguration.yaml", "name: MyExample", $"name: {name}");
            vsm.ReplaceContentInFile($"{_artifact.GetPath(_artifact.Source.RenamedCommandsProject)}\\Configuration\\PowerCommandsConfiguration.cs", "MyExampleCommands", $"{name}Commands");
            vsm.ReplaceContentInFile($"{_artifact.GetPath(_artifact.Source.RenamedCommandsProject)}\\GlobalUsings.cs", "MyExampleCommands", $"{name}Commands");
            vsm.ReplaceContentInFile($"{_artifact.GetPath(_artifact.Source.RenamedCommandsProject)}\\PowerCommandServices.cs", "MyExampleCommands", $"{name}Commands");
            foreach (var cmdName in _artifact.Commands) vsm.ReplaceContentInFile($"{_artifact.GetPath(_artifact.Source.RenamedCommandsProject)}\\Commands\\{cmdName}Command.cs", "MyExampleCommands", $"{name}Commands");
            foreach (var cmdName in _artifact.TemplateCommands)
            {
                vsm.ReplaceContentInFile($"{_artifact.GetPath(_artifact.Source.RenamedCommandsProject)}\\PainKiller.PowerCommands.MyExampleCommands.csproj", $@"<ItemGroup>
    <Compile Remove=""Commands\Templates\{cmdName}Command.cs"" />
  </ItemGroup>", $"");
                vsm.ReplaceContentInFile($"{_artifact.GetPath(_artifact.Source.RenamedCommandsProject)}\\PainKiller.PowerCommands.MyExampleCommands.csproj", $@"<ItemGroup>
    <None Include=""Commands\Templates\{cmdName}Command.cs"" />
  </ItemGroup>", $"");
            }

            vsm.MoveFile($"{_artifact.GetPath(_artifact.Source.RenamedCommandsProject)}\\PainKiller.PowerCommands.MyExampleCommands.csproj", $"{_artifact.GetPath(_artifact.Source.RenamedCommandsProject)}\\PainKiller.PowerCommands.{name}Commands.csproj");

            vsm.MoveDirectory(_artifact.Source.Core, _artifact.Target.Core);
            vsm.MoveDirectory(_artifact.Source.BootstrapProject, _artifact.Target.BootstrapProject);
            vsm.MoveDirectory(_artifact.Source.ConsoleProject, _artifact.Target.ConsoleProject);
            vsm.MoveDirectory(_artifact.Source.ThirdParty, _artifact.Target.ThirdParty);
            vsm.MoveFile($"{_artifact.GetPath(_artifact.Source.SolutionFileName)}", $"{_artifact.GetPath(_artifact.Target.SolutionFileName)}");
            foreach (var cmdName in _artifact.Commands) vsm.MoveFile($"{_artifact.GetPath(_artifact.Source.RenamedCommandsProject)}\\Commands\\{cmdName}Command.cs", $"{cmdName}Command.cs");
            vsm.MoveDirectory($"{_artifact.GetPath(_artifact.Source.RenamedCommandsProject)}", $"{_artifact.GetPath(_artifact.Target.CommandsProject)}");
            vsm.DeleteDir($"{_artifact.GetPath(_artifact.Target.CommandsProject)}\\Commands");
            vsm.CreateDirectory($"{_artifact.GetPath(_artifact.Target.CommandsProject)}\\Commands");
            foreach (var cmdName in _artifact.Commands) vsm.MoveFile($"{cmdName}Command.cs", $"{_artifact.GetPath(_artifact.Target.CommandsProject)}\\Commands\\{cmdName}Command.cs");

            vsm.DeleteDownloadsDirectory();

            WriteHeadLine("\nAll work is done, now do the following steps");
            WriteHeadLine("\n1. Set the PowerCommandsConsole project as startup project");
            WriteHeadLine("\n2. Build and then run the solution");
            WriteHeadLine("\n3. type demo and then hit enter to see that it is all working");
            WriteHeadLine("\nNow you are ready to add your commands, read more about that on github:\nhttps://github.com/PowerCommands/PowerCommands2022/blob/main/PowerCommands%20Design%20Principles%20And%20Guidlines.md");

            ShellService.Service.OpenDirectory(_path);
            return Ok();
        }
    }
}