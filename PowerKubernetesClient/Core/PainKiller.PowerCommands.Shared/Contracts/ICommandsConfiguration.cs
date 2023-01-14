using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;

namespace PainKiller.PowerCommands.Shared.Contracts
{
    public interface ICommandsConfiguration
    {
        bool ShowDiagnosticInformation { get; set; }
        string DefaultCommand { get; set; }
        public string CodeEditor { get; set; }
        string Repository { get; set; }
        string BackupPath { get; set; }
        Metadata Metadata { get; set; }
        LogComponentConfiguration Log { get; set; }
        List<BaseComponentConfiguration> Components { get; set; }
        SecretConfiguration Secret { get; set; }
        EnvironmentConfiguration Environment { get; set; }
    }
}