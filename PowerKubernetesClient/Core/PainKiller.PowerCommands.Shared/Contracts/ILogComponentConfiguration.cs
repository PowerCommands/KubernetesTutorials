namespace PainKiller.PowerCommands.Shared.Contracts
{
    public interface ILogComponentConfiguration
    {
        string FileName { get; set; }
        string FilePath { get; set; }
        string RollingIntervall { get; set; }
        string RestrictedToMinimumLevel { get; set; }
    }
}