using PainKiller.PowerCommands.Configuration.DomainObjects;

namespace PainKiller.PowerCommands.Configuration.Contracts
{
    public interface IConfigurationService
    {
        YamlContainer<T> Get<T>(string inputFileName = "") where T : new();
        string SaveChanges<T>(T configuration, string inputFileName = "") where T : new();
        void Create<T>(T configuration, string fullFileName) where T : new();
        YamlContainer<T> GetAppDataConfiguration<T>(T defaultIfMissing, string inputFileName = "") where T : new();
    }
}