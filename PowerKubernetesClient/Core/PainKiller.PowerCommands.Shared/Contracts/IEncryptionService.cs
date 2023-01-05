using System.Security.Cryptography;

namespace PainKiller.PowerCommands.Shared.Contracts;

public interface IEncryptionService
{
    string EncryptString(string plainText);
    string DecryptString(string plainText);
    public static string GetRandomSalt()
    {
        var data = new byte[32];
        for (var i = 0; i < 10; i++) RandomNumberGenerator.Fill(data);
        var retVal = Convert.ToBase64String(data);
        return retVal;
    }
}