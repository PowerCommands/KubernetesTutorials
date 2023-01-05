using PainKiller.PowerCommands.Shared.Contracts;

namespace PainKiller.PowerCommands.Shared.Utils.DisplayTable;

public class ColumnRenderHeader : ColumnRenderBase
{
    public ColumnRenderHeader(IConsoleWriter consoleWriter) : base(consoleWriter) {}
    public override void Write(string value) => ConsoleWriter.Write(value + "|", color:ConsoleColor.Blue);
}