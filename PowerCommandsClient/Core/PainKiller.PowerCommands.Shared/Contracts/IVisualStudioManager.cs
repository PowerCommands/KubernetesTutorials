namespace PainKiller.PowerCommands.Shared.Contracts;

public interface IVisualStudioManager
{
    void CreateRootDirectory(bool onlyRepoSrcCodeRootPath = false);
    void CreateDirectory(string name);
    void CreateDownloadsDirectory();
    void DeleteDownloadsDirectory();
    void CloneRepo(string repo);
    void DeleteDir(string directory);
    void DeleteFile(string fileName, bool repoFile);
    void RenameDirectory(string directory, string name);
    void MoveDirectory(string dirctoryName, string toDirctoryName);
    string BackupDirectory(string dirctoryName);
    void MoveFile(string fileName, string toFileName);
    void WriteNewSolutionFile(string[] validProjectFiles);
    void ReplaceContentInFile(string fileName, string find, string replace);
    void MergeDocsDB();
}