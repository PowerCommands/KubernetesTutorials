namespace PainKiller.PowerCommands.Shared.Contracts;

public interface IDiagnosticManager
{
    bool ShowDiagnostic { get; set; }
    bool ShowElapsedTime { get; set; }
    void Message(string diagnostic);
    void Header(string header);
    void Warning(string warning);
    void Start();
    void Stop();
    string RootPath();
}