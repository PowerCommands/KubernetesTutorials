using PainKiller.PowerCommands.Shared.Enums;

namespace PainKiller.PowerCommands.Shared.DomainObjects.Core
{
    public class ProxyResult
    {
        public string Identifier { get; set; } = "";
        public string Raw { get; set; } = "";
        public string Output { get; set; } = "";
        public RunResultStatus Status { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
    }
}