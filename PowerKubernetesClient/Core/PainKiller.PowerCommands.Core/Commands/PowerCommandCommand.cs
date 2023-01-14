namespace PainKiller.PowerCommands.Core.Commands
{
    [PowerCommandDesign(description: "Create or update the Visual Studio Solution with all depended projects",
                            arguments: "!<action> (new or update)",
                              options: "command|solution|output|template|backup",
                           suggestions: "new",
                               quotes: "<path>",
                   disableProxyOutput: true,
                              example: "//create new VS solution|powercommand new --solution testproject --output \"C:\\Temp\\\"|//Create new PowerCommand named Demo|powercommand new --command Demo|//Update powercommands core, this will first delete current Core projects and than apply the new Core projects|powercommand update|//Only update template(s)|powercommand update --templates|//Update with backup|powercommand update --backup|//Create a new command|powercommand new --command MyNewCommand")]
    public class PowerCommandCommand : CommandBase<CommandsConfiguration>
    {
        private readonly ArtifactPathsConfiguration _artifact = ConfigurationService.Service.Get<ArtifactPathsConfiguration>().Configuration;
        public PowerCommandCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }
        public override RunResult Run()
        {
            var name = Input.HasOption("solution") ? Input.GetOptionValue("solution") : Input.GetOptionValue("template");
            _artifact.Name = name;

            if (Input.SingleArgument == "new" && Input.HasOption("solution") && !string.IsNullOrEmpty(Input.GetOptionValue("solution")) && Input.HasOption("output") && !string.IsNullOrEmpty(Input.GetOptionValue("output")))
            {
                var cmdNew = new CommandNewSolution(Identifier, Configuration, _artifact, Input);
                return cmdNew.Run();
            }
            if (Input.SingleArgument == "update")
            {
                var cmdUpdate = new CommandUpdate(Identifier, Configuration, _artifact, Input);
                return cmdUpdate.Run();
            }
            if (Input.HasOption("template") && !string.IsNullOrEmpty(Input.GetOptionValue("template")))
            {
                BadParameterError("Not implemented yet...");
            }
            if (Input.SingleArgument == "new" && Input.HasOption("command")) return CreateCommand(Input.GetOptionValue("command"));
            return BadParameterError("Missing arguments");
        }

        private RunResult CreateCommand(string name)
        {
            ITemplateManager templateManager = new TemplateManager(name, WriteLine);
            templateManager.CreateCommand(templateName: "Default", name);
            return Ok();
        }

        protected void UpdateTemplates(IVisualStudioManager vsm, bool cloneRepo = false, string newProjectName = "")
        {
            if (cloneRepo)
            {
                vsm.DeleteDownloadsDirectory();
                vsm.CreateDownloadsDirectory();
                vsm.CloneRepo(Configuration.Repository);
            }

            var name = string.IsNullOrEmpty(newProjectName) ? VisualStudioManager.GetName() : newProjectName;
            var templateManager = new TemplateManager(name, WriteLine);
            templateManager.InitializeTemplatesDirectory();
            templateManager.CopyTemplates();

            vsm.MergeDocsDB();
            vsm.DeleteFile(_artifact.DocsDbFile, repoFile: true);
        }
    }
}