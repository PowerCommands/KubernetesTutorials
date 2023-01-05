namespace PainKiller.PowerCommands.Configuration.DomainObjects;

public class YamlContainer<T> where T: new()
{
    public string Version { get; set; } = "";
    public T Configuration { get; set; } = new();
}