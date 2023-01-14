using PainKiller.PowerCommands.Shared.Contracts;
using PainKiller.PowerCommands.Shared.Utils.DisplayTable;

namespace PainKiller.PowerCommands.Shared.DomainObjects.Core
{
    public class CommandTestReport : IConsoleCommandTable
    {
        [ColumnRenderOptions(caption: "TestDisabled", order: 0)]
        public bool TestDisabled { get; set; } = true;

        [ColumnRenderOptions(caption: "Command", order: 1)]
        public string Command { get; set; } = "Missing PowerCommandTest attribute";

        [ColumnRenderOptions(caption: "Tests", order: 2)]
        public int Tests { get; set; }

        [ColumnRenderOptions(caption: "Errors", order: 3)]
        public int Failures { get; set; }

        [ColumnRenderOptions(caption: "Result", order: 4, renderFormat: ColumnRenderFormat.SucessOrFailure, trigger1: "*SUCCESS*", trigger2: "*FAILURE*", mark: "*")]
        public string Result => TestDisabled ? "Unknown" : Failures > 0 ? "*FAILURE*" : "*SUCCESS*";
    }
}