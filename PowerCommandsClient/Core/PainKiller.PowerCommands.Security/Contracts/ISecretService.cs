namespace PainKiller.PowerCommands.Security.Contracts;

public interface ISecretService
{
    string GetSecret(string name, Dictionary<string, string> options, Func<string,string> decryptFunction);
    string SetSecret(string name, string secret, Dictionary<string, string> options, Func<string, string> encryptFunction);
    string ReplaceSecret(string content, string name, Dictionary<string, string> options, Func<string, string> decryptFunction);
}