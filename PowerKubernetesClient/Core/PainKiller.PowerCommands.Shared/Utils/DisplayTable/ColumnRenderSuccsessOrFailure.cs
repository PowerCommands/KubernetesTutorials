using PainKiller.PowerCommands.Shared.Contracts;

namespace PainKiller.PowerCommands.Shared.Utils.DisplayTable;

public class ColumnRenderSuccsessOrFailure : ColumnRenderBase
{
    private readonly string _successTrigger;
    private readonly string _failureTrigger;
    private readonly string _marker;

    public ColumnRenderSuccsessOrFailure(IConsoleWriter consoleWriter, string successTrigger, string failureTrigger, string marker) : base (consoleWriter)
    {
        _successTrigger = successTrigger;
        _failureTrigger = failureTrigger;
        _marker = marker;
    }
    public override void Write(string value)
    {
        if(value.Contains(_successTrigger)) ConsoleWriter.WriteSuccess($"{value.Replace(_marker, " ")}");
        else if (value.Contains(_failureTrigger)) ConsoleWriter.WriteFailure($"{value.Replace(_marker, " ")}");
        else ConsoleWriter.Write(value);
        ConsoleWriter.Write("|");
    }
}