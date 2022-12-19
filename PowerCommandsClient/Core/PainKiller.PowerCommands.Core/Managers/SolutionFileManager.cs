namespace PainKiller.PowerCommands.Core.Managers;
public class SolutionFileManager : ISolutionFileManager
{
    private readonly string _path;
    private readonly List<VSSolutionProjectReference> _goodProjectReferences = new();
    public SolutionFileManager(string path) => _path = path;
    public void WriteValidProjectFiles(string name, string[] validProjectFiles, string solutionFileTarget)
    {
        var contentRows = File.ReadAllLines(_path);

        var validProjectsRows = new List<string>();
        foreach (var row in contentRows)
        {
            if (row.Trim().StartsWith("EndProject")) continue;
            if (row.Trim().StartsWith("Project("))
            {
                var vssProjectRef = new VSSolutionProjectReference(row);
                if (validProjectFiles.All(p => p != vssProjectRef.ProjectFilePath)) continue;
                if(vssProjectRef.ProjectPath.Contains("Third party components")) _goodProjectReferences.Add(vssProjectRef);
                if(vssProjectRef.ProjectPath.StartsWith("Core")) _goodProjectReferences.Add(vssProjectRef);
                if (vssProjectRef.ProjectFilePath == "PainKiller.PowerCommands.MyExampleCommands\\PainKiller.PowerCommands.MyExampleCommands.csproj")
                {
                    var newProjectRow = row.Replace("MyExample", name);
                    validProjectsRows.Add($"{newProjectRow}\nEndProject\n");
                    continue;
                }
                validProjectsRows.Add($"{row}\nEndProject\n");
                continue;
            }
            validProjectsRows.Add(row);
        }
        File.WriteAllLines(solutionFileTarget, validProjectsRows);
    }
    public void RemoveGlobalSectionNestedProjects(string name, string solutionFileTarget)
    {
        var contentRows = File.ReadAllLines(solutionFileTarget);
        var validProjectsRows = new List<string>();

        var startSkip = false;
        foreach (var row in contentRows)
        {
            if (row.Contains("GlobalSection(ExtensibilityGlobals)")) startSkip = false;
            if (row.Contains("GlobalSection(NestedProjects) = preSolution") || startSkip)
            {
                startSkip = true;
                if (_goodProjectReferences.Any(r => row.Contains(r.ProjectIdentifier.Trim().Replace("\"", ""))))
                {
                    validProjectsRows.Add(row);
                    continue;
                }
                if (!row.Contains("GlobalSection(NestedProjects) = preSolution") && !row.Contains("EndGlobalSection")) continue;
                
                
            }
            validProjectsRows.Add(row);
        }
        File.WriteAllLines(solutionFileTarget, validProjectsRows);
    }
    public static string GetLocalSolutionRoot()
    {
        var parts = AppContext.BaseDirectory.Split('\\');
        var endToRemove = $"\\{parts[^5]}\\{parts[^4]}\\{parts[^3]}\\{parts[^2]}";
        return AppContext.BaseDirectory.Replace(endToRemove, "");
    }
}