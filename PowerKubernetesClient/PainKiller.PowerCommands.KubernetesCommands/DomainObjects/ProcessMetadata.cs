namespace PainKiller.PowerCommands.KubernetesCommands.DomainObjects;
public class ProcessMetadata
{
    public string Name { get; set; } = "";
    public string Args { get; set; } = "";
    public string Description { get; set; } = "";
    public string Url { get; set; } = "";
    public bool UseShellExecute { get; set; }
    public bool WaitForExit { get; set; }
    public bool DisableOutputLogging { get; set; }
    public bool Base64Decode { get; set; }
    public int WaitSec { get; set; }
    public bool UseReadline { get; set; }
}