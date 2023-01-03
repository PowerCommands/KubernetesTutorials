using System.Diagnostics;

namespace PainKiller.PowerCommands.KubernetesCommands.Commands;

[PowerCommandTest(         tests: " ")]
[PowerCommandDesign( description: "Description of your command...",
                         example: "demo")]
public class ProxyCommand : CommandBase<PowerCommandsConfiguration>
{
    private static Process? _proxy = null;
    private static StreamWriter? _stdInput;
    public ProxyCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        if (_proxy == null)
        {
            _proxy = ShellService.Service.GetProxyConsoleProcess("C:\\Repos\\Github\\KubernetesTutorials\\PowerCommandsClient\\DummieConsole\\bin\\Debug\\net7.0\\DummieConsole", arguments: "", workingDirectory: "C:\\Repos\\Github\\KubernetesTutorials\\PowerCommandsClient\\DummieConsole\\bin\\Debug\\net7.0");

            _proxy.EnableRaisingEvents = true;
            _proxy.OutputDataReceived += Process_OutputDataReceived;
            
            _proxy.Start();
            _proxy.BeginOutputReadLine();

            _stdInput = _proxy.StandardInput;
        }
        _stdInput?.WriteLine(Input.SingleArgument);
        return Ok();
    }
    private void Process_OutputDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
    {
        Console.WriteLine(e.Data);
    }
}