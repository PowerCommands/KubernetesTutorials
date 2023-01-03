namespace PainKiller.PowerCommands.KubernetesCommands.Commands;

[PowerCommandTest(         tests: " ")]
[PowerCommandDesign( description: "Description of your command...",
                         example: "demo")]
public class KCommand : CommandBase<PowerCommandsConfiguration>
{
    public KCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        //https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.processstartinfo.redirectstandardoutput?redirectedfrom=MSDN&view=net-7.0#System_Diagnostics_ProcessStartInfo_RedirectStandardOutput
        //https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.processstartinfo.redirectstandardinput?redirectedfrom=MSDN&view=net-7.0#System_Diagnostics_ProcessStartInfo_RedirectStandardInput

        ShellService.Service.Execute("kubectl", Input.Raw.Replace($"{Input.Identifier} ","").Replace("--no-quit",""), "", ReadLine, "", waitForExit: true);
        if (!string.IsNullOrEmpty(LastReadLine)) WriteLine(LastReadLine);
        return Ok();
    }
}