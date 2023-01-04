using System.Text;

namespace PainKiller.PowerCommands.KubernetesCommands.Commands;

[PowerCommandTest(         tests: "--decode P@ssword1$")]
[PowerCommandDesign( description: "Base64 encode or decode string content",
                         options: "!secret|data|!encode|!decode",
                         example: "base64 --decode P@ssword1$")]
public class Base64Command : CommandBase<PowerCommandsConfiguration>
{
    public Base64Command(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        var encode = GetOptionValue("encode");
        var decode = GetOptionValue("decode");
        if(!string.IsNullOrEmpty(encode)) WriteLine($"Encoded string: {Convert.ToBase64String(Encoding.UTF8.GetBytes(encode))}");
        if(!string.IsNullOrEmpty(decode)) WriteLine($"Decoded string: {Encoding.UTF8.GetString(Convert.FromBase64String(decode))}");
        if (HasOption("secret")) DecodeSecret(GetOptionValue("secret"), GetOptionValue("data"));
        return Ok();
    }
    private void DecodeSecret(string secret, string data)
    {
        WriteCodeExample("kubectl",$"config get secret {secret} -ojsonpath='{{.data.{data}}}'");
        ShellService.Service.Execute("kubectl",$"get secret {secret} -ojsonpath='{{.data.{data}}}'","", ReadLine,"", waitForExit: true);
        var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(LastReadLine.Replace("'","")));
        Console.WriteLine(decoded);
    }
}