namespace PainKiller.PowerCommands.Security.Contracts
{
    public interface IEncryptionManager
    {
        string EncryptString(string plainText, string sharedSecret);
        string DecryptString(string cipherText, string sharedSecret);
    }
}