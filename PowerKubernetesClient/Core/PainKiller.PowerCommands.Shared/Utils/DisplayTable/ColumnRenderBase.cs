using PainKiller.PowerCommands.Shared.Contracts;

namespace PainKiller.PowerCommands.Shared.Utils.DisplayTable
{
    public class ColumnRenderBase : IColumnRender
    {
        protected readonly IConsoleWriter ConsoleWriter;
        public ColumnRenderBase(IConsoleWriter consoleWriter) => ConsoleWriter = consoleWriter;
        public virtual void Write(string value) => ConsoleWriter.Write("");
    }
}