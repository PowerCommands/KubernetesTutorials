namespace PainKiller.PowerCommands.Shared.Contracts;

public interface ITemplateManager
{
    void InitializeTemplatesDirectory();
    void CopyTemplates();
    void CreateCommand(string templateName, string commandName);
}