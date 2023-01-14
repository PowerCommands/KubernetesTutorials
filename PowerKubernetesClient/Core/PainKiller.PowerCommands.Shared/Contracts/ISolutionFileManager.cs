namespace PainKiller.PowerCommands.Shared.Contracts
{
    public interface ISolutionFileManager
    {
        void WriteValidProjectFiles(string name, string[] validProjectFiles, string solutionFileTarget);
        void RemoveGlobalSectionNestedProjects(string name, string solutionFileTarget);
    }
}