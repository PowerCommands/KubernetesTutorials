namespace PainKiller.PowerCommands.Shared.DomainObjects.Core
{
    public class PowerOption
    {
        public PowerOption(string attributeValue)
        {
            IsRequired = attributeValue.StartsWith("!");
            Name = attributeValue.Replace("!", "");
        }
        public string Name { get; set; }
        public string Value { get; set; } = "";
        public string Raw => $"--{Name}";
        public bool IsRequired { get; set; }
    }
}