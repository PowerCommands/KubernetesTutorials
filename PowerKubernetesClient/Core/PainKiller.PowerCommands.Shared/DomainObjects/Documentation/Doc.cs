using PainKiller.PowerCommands.Shared.Contracts;

namespace PainKiller.PowerCommands.Shared.DomainObjects.Documentation
{
    public class Doc
    {
        public int DocID { get; set; }
        public string Name { get; set; } = "";
        public string Uri { get; set; } = "";
        public DateTime Updated { get; set; } = DateTime.Now;
        public string Tags { get; set; } = "";
        public int Version { get; set; }
    }
}