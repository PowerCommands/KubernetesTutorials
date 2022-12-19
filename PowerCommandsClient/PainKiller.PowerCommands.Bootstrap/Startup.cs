using Microsoft.Extensions.Logging;
using PainKiller.PowerCommands.Core.Managers;
using PainKiller.PowerCommands.Core.Services;
using PainKiller.PowerCommands.KubernetesCommands;
using PainKiller.PowerCommands.KubernetesCommands.Configuration;

namespace PainKiller.PowerCommands.Bootstrap;
public static class Startup
{
    public static PowerCommandsManager ConfigureServices()
    {
        var services = PowerCommandServices.Service;
        
        services.Configuration.Environment.InitializeValues();
        services.Logger.LogInformation("Program started, configuration read");

        var componentManager = new ComponentManager<PowerCommandsConfiguration>(services.ExtendedConfiguration, services.Diagnostic);
        try
        {
            var validatePlugins = componentManager.ValidateConfigurationWithComponents();
            if (!validatePlugins)
            {
                services.Diagnostic.Warning("\nWarning, some of the components has an invalid checksum in the configuration file");
                services.Diagnostic.Message("If you continuously working with your Commands, that is ok, when you are distribute your application you should update checksum to match does dll files that ju distribute.\nYou could use the ChecksumCommand class in KubernetesCommands on github to calculate the checksum(MDE5Hash).");
            }
        }
        catch (Exception ex)
        {
            services.Logger.LogCritical(ex, "Critical error, program could not start");
            throw;
        }
        ConsoleService.Service.WriteLine(nameof(Startup), "\nUse the tab key to cycle trough available commands, use <command name> --help or describe <search phrase> to display documentation.", null);
        ConsoleService.Service.WriteLine(nameof(Startup), "\nUse up or down key  to cycle trough command history.", null);
        return new PowerCommandsManager(services);
    }
}