namespace PainKiller.PowerCommands.Shared.DomainObjects.Configuration;

public class EnvironmentConfiguration
{
    private readonly Dictionary<string, string?> _values = new();
    public List<EnvironmentItemConfiguration>? Variables { get; set; } = new();

    public void InitializeValues()
    {
        if(Variables == null) return;
        foreach (var variable in Variables)
        {
            var target = string.IsNullOrEmpty(variable.EnvironmentVariableTarget) ? EnvironmentVariableTarget.User : Enum.Parse<EnvironmentVariableTarget>(variable.EnvironmentVariableTarget);
            var envVariableValue = Environment.GetEnvironmentVariable(variable.Name, target);
            if(!_values.ContainsKey(variable.Name)) _values.Add(variable.Name, $"{envVariableValue}");
        }
    }
    public string GetValue(string environmentVariableName)
    {
        _values.TryGetValue(environmentVariableName, out string? envValue);
        return string.IsNullOrEmpty(envValue) ? "" : envValue;
    }
    public Dictionary<string, string?> GetValues() => _values;
}