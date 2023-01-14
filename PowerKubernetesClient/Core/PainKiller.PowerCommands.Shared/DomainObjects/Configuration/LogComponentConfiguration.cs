using PainKiller.PowerCommands.Shared.Contracts;

namespace PainKiller.PowerCommands.Shared.DomainObjects.Configuration
{
    public class LogComponentConfiguration : BaseComponentConfiguration, ILogComponentConfiguration
    {
        public LogComponentConfiguration()
        {
            Name = "Serialog";
            Component = "PainKiller.SerilogExtensions.dll";
            Checksum = "173831af7e77b8bd33e32fce0b4e646d";
        }
        public string FileName { get; set; } = "powercommands.log";
        public string FilePath { get; set; } = "logs";
        public string RollingIntervall { get; set; } = "Day";
        public string RestrictedToMinimumLevel { get; set; } = "Information";
    }
}