namespace PainKiller.PowerCommands.Shared.DomainObjects.Core;

/// <summary>
///                     Project Reference                                   Project Path                                        Project File Path                                                Project Identifier 
/// Example Project("{9A19103F-16F7-4668-BE54-9A1E7A4F7556}") = "PainKiller.PowerCommands.Bootstrap", "PainKiller.PowerCommands.Bootstrap\PainKiller.PowerCommands.Bootstrap.csproj", "{0902370A-5117-4599-87D7-7A117E36E41E}"
/// </summary>
public class VSSolutionProjectReference
{
    public VSSolutionProjectReference(){}
    public VSSolutionProjectReference(string row)
    {
        //Project("{9A19103F-16F7-4668-BE54-9A1E7A4F7556}")
        var rawData = row.Split('=');
        ProjectReference = rawData.First().Substring(rawData.First().IndexOf('{')+1, rawData.First().IndexOf('}')-1 - rawData.First().IndexOf('{'));
        ProjectPath = rawData[1].Split(',').First().Trim().Replace("\"", "");
        ProjectFilePath = rawData[1].Split(',')[1].Trim().Replace("\"", "");
        ProjectIdentifier = rawData[1].Split(',')[2].Replace("{", "").Replace("}", "");
    }
    public string ProjectReference { get; set; } = "";
    public string ProjectPath { get; set; } = "";
    public string ProjectFilePath { get; set; } = "";
    public string ProjectIdentifier { get; set; } = "";
}