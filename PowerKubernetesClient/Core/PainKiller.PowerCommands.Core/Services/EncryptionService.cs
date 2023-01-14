namespace PainKiller.PowerCommands.Core.Services
{
    public class EncryptionService : IEncryptionService
    {
        private EncryptionService()
        {
            var securityConfiguration = ConfigurationService.Service.GetAppDataConfiguration(new SecurityConfiguration { Encryption = new EncryptionConfiguration { SharedSecretEnvironmentKey = nameof(_encryptionManager), SharedSecretSalt = IEncryptionService.GetRandomSalt() } }, ConfigurationGlobals.SecurityFileName).Configuration;
            _salt = securityConfiguration.Encryption.SharedSecretSalt;
            _sharedSecret = Environment.GetEnvironmentVariable(securityConfiguration.Encryption.SharedSecretEnvironmentKey, EnvironmentVariableTarget.User) ?? string.Empty;
            if (string.IsNullOrEmpty(_sharedSecret)) ConsoleService.Service.WriteWarning(nameof(EncryptionService), $"The environment variable [{securityConfiguration.Encryption.SharedSecretEnvironmentKey}] is missing, you should generate a salt with secret --salt\nAnd then create the environment variable with the salt as value with user scope, before you create secrets (otherwise you need to re-create them again)");
            _encryptionManager = new AESEncryptionManager(_salt);
        }

        private static readonly Lazy<IEncryptionService> Lazy = new(() => new EncryptionService());
        public static IEncryptionService Service => Lazy.Value;

        private readonly string _sharedSecret;
        private readonly string _salt;
        private readonly IEncryptionManager _encryptionManager;
        public string EncryptString(string plainText)
        {
            var encryptionManager = new AESEncryptionManager(_salt);
            var retVal = encryptionManager.EncryptString(plainText, _sharedSecret);
            return retVal;
        }
        public string DecryptString(string plainText)
        {
            var encryptionManager = new AESEncryptionManager(_salt);
            var retVal = encryptionManager.DecryptString(plainText, _sharedSecret);
            return retVal;
        }
    }
}