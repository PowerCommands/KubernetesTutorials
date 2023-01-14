namespace PainKiller.PowerCommands.Shared.DomainObjects.Configuration
{
    public class ProxyPowerCommandConfiguration
    {
        public string Name { get; set; } = "";
        public string WorkingDirctory { get; set; } = "";
        public List<string> Commands { get; set; } = new();
    }
}