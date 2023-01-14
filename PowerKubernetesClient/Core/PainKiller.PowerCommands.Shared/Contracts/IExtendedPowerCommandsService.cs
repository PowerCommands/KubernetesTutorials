namespace PainKiller.PowerCommands.Shared.Contracts
{
    public interface IExtendedPowerCommandServices<out TConfiguration> : IPowerCommandServices where TConfiguration : ICommandsConfiguration
    {
        TConfiguration ExtendedConfiguration { get; }
    }
}