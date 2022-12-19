using PainKiller.PowerCommands.Security.Contracts;

namespace PainKiller.PowerCommands.Security.Services;

public class PasswordPromptService : IPasswordPromptService
{
    private PasswordPromptService() { }
    private static readonly Lazy<IPasswordPromptService> Lazy = new(() => new PasswordPromptService());
    public static IPasswordPromptService Service => Lazy.Value;
    public string ReadPassword()
    {
        var pass = string.Empty;
        ConsoleKey key;
        do
        {
            var keyInfo = Console.ReadKey(intercept: true);
            key = keyInfo.Key;

            if (key == ConsoleKey.Backspace && pass.Length > 0)
            {
                Console.Write("\b \b");
                pass = pass[0..^1];
            }
            else if (!char.IsControl(keyInfo.KeyChar))
            {
                Console.Write("*");
                pass += keyInfo.KeyChar;
            }
        } while (key != ConsoleKey.Enter);
        return pass;
    }
}