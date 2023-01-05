using PainKiller.PowerCommands.Shared.Contracts;

namespace PainKiller.PowerCommands.Shared.Utils.DisplayTable;

public class ColumnRenderStandard : ColumnRenderBase
{
    public ColumnRenderStandard(IConsoleWriter consoleWriter) : base(consoleWriter) { }
    public override void Write(string value) => ConsoleWriter.Write($"{value}|");
}