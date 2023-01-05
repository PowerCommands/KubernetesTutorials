namespace PainKiller.PowerCommands.Core.Managers;
public class ComponentManager<TConfiguration> where TConfiguration : CommandsConfiguration
{
    private readonly TConfiguration _configuration;
    private readonly IDiagnosticManager _diagnostic;
    public ComponentManager(TConfiguration configuration, IDiagnosticManager diagnostic)
    {
        _configuration = configuration;
        _diagnostic = diagnostic;
    }
    public bool ValidateConfigurationWithComponents()
    {
        var retVal = true;
        var components = typeof(TConfiguration).GetPropertiesOfT<BaseComponentConfiguration>().Select(c => c.GetValue(_configuration) as BaseComponentConfiguration).ToList();
        if(_configuration.Components.Count > 0) components.AddRange(_configuration.Components);
        _diagnostic.Header("\nChecksum validations:");
        foreach (var component in components)
        {
            if(component is null) continue;
            var fileCheckSum = new FileChecksum(component.Component);
            var validateCheckSum = fileCheckSum.CompareFileChecksum(component.Checksum) ? "Valid" : "Not valid";
            if(validateCheckSum == "Not valid") retVal = false;
            _diagnostic.Message($"{component.Name} Checksum {fileCheckSum.Mde5Hash} {validateCheckSum}");
        }
        return retVal;
    }
    public List<BaseComponentConfiguration> AutofixConfigurationComponents<T>(T configuration) where T : CommandsConfiguration, new()
    {
        var retVal = new List<BaseComponentConfiguration>();
        foreach (var component in configuration.Components)
        {
            component.Checksum = new FileChecksum(component.Component).Mde5Hash;
            retVal.Add(component);
        }
        return retVal;
    }
}