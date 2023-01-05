using Microsoft.Extensions.Logging;
using PainKiller.PowerCommands.Configuration;
using PainKiller.PowerCommands.Security.DomainObjects;
using PainKiller.PowerCommands.Shared.Contracts;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;

namespace PainKiller.PowerCommands.Bootstrap.Extensions;

public static class ServiceConfigurationExtensions
{
    public static TPowerCommandServices ShowDiagnostic<TPowerCommandServices>(this TPowerCommandServices services, bool showDiagnostic) where TPowerCommandServices : IPowerCommandServices
    {
        services.Configuration.ShowDiagnosticInformation = showDiagnostic;
        services.Diagnostic.ShowDiagnostic = showDiagnostic;
        return services;
    }
    public static TPowerCommandServices SetMetadata<TPowerCommandServices>(this TPowerCommandServices services, Metadata metadata) where TPowerCommandServices : IPowerCommandServices
    {
        services.Configuration.Metadata = metadata;
        return services;
    }
    /// <summary>Change must be persisted, restart needed</summary>
    public static TPowerCommandServices SetLogMinimumLevel<TPowerCommandServices>(this TPowerCommandServices services, LogLevel logLevel) where TPowerCommandServices : IPowerCommandServices
    {
        services.Configuration.Log.RestrictedToMinimumLevel = logLevel.ToString();
        return services;
    }
    public static TPowerCommandServices AddComponent<TPowerCommandServices>(this TPowerCommandServices services, string name, string assemblyName) where TPowerCommandServices : IPowerCommandServices
    {
        var fileCheckSum = new FileChecksum(assemblyName).Mde5Hash;
        services.Configuration.Components.Add(new BaseComponentConfiguration{Checksum = fileCheckSum,Component = assemblyName,Name = name});
        return services;
    }
    public static TPowerCommandServices PersistChanges<TPowerCommandServices>(this TPowerCommandServices services) where TPowerCommandServices : IPowerCommandServices
    {
        ConfigurationService.Service.SaveChanges((object) services.Configuration, "");
        return services;
    }
}