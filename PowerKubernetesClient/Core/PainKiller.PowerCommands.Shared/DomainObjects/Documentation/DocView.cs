namespace PainKiller.PowerCommands.Shared.DomainObjects.Documentation;

public class DocView
{
    private string _tags = "";
    public int ID { get; set; }
    public string Name { get; set; } = "";
    public string Tags
    {
        get => _tags;
        set => _tags = value.Length > 70 ? $"{value.Substring(0, 70)}..." : value;
    }
    public int Ver { get; set; }
}