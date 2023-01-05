using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;

namespace PainKiller.PowerCommands.Shared.Extensions;

public static class ConfigurationExtensions
{
    public static void AddEnvironmentVariable(this EnvironmentConfiguration configuration, string name, string val, EnvironmentVariableTarget target = EnvironmentVariableTarget.User)
    {
        if (configuration.Variables != null) configuration.Variables.Add(new EnvironmentItemConfiguration { EnvironmentVariableTarget = $"{target}", Name = name });
        else configuration.Variables = new List<EnvironmentItemConfiguration> { new() { EnvironmentVariableTarget = $"{EnvironmentVariableTarget.User}", Name = name } };
        if(string.IsNullOrEmpty(Environment.GetEnvironmentVariable("name"))) Environment.SetEnvironmentVariable(name, val, target);
    }
}