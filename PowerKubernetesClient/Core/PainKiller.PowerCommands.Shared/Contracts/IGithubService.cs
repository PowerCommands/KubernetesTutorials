namespace PainKiller.PowerCommands.Shared.Contracts
{
    public interface IGithubService
    {
        void MergeDocsDB();
        void DownloadCommand(string commandName);
    }
}