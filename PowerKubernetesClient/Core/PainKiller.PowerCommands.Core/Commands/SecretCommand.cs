using PainKiller.PowerCommands.Security.Services;

namespace PainKiller.PowerCommands.Core.Commands
{
    [PowerCommandDesign(description: "Get, creates, removes or view secrets, first you need to configure your encryption key with initialize argument",
                            options: "initialize|configuration|create|get|remove|salt",
                 disableProxyOutput: true,
                            example: "//View all declared secrets|secret|//Get the decrypted value of named secret|secret --get \"mycommand-pass\"|secret --create \"mycommand-pass\"|secret --remove \"mycommand-pass\"|//Initialize your machine with a new encryption key (stops if this is already done)|secret --initialize")]
    public class SecretCommand : CommandBase<CommandsConfiguration>
    {
        public SecretCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }
        public override RunResult Run()
        {
            if (Input.HasOption("initialize")) return Init();
            if (Input.HasOption("")) return CheckEncryptConfiguration();
            if (Input.HasOption("salt")) return Salt();
            if (Input.HasOption("get")) return Get();
            if (Input.HasOption("create")) return Create();
            if (Input.HasOption("remove")) return Remove();
            if ((Input.Arguments.Length + Input.Quotes.Length < 2) && Input.Arguments.Length > 0) throw new MissingFieldException("Two parameters must be provided");
            if (Input.Arguments.Length == 0 || Input.Arguments[0] == "view") return List();

            return BadParameterError("No matching parameter");
        }
        private RunResult CheckEncryptConfiguration()
        {
            try
            {
                var encryptedString = EncryptionService.Service.EncryptString("Encryption is setup properly");
                var decryptedString = EncryptionService.Service.DecryptString(encryptedString);
                WriteLine(encryptedString);
                WriteLine(decryptedString);
            }
            catch
            {
                Console.WriteLine("");
                WriteError("Encryption is not configured properly");
            }
            return Ok();
        }
        private RunResult Salt()
        {
            Console.WriteLine(IEncryptionService.GetRandomSalt());
            return Ok();
        }

        private RunResult Init()
        {
            var salt = IEncryptionService.GetRandomSalt();
            WriteLine($"{salt.Length}");
            var firstHalf = salt.Substring(0, 22);
            var secondHalf = salt.Substring(22, 22);
            Environment.SetEnvironmentVariable("_encryptionManager", firstHalf, EnvironmentVariableTarget.User);
            var securityConfig = new SecurityConfiguration { Encryption = new EncryptionConfiguration { SharedSecretEnvironmentKey = "_encryptionManager", SharedSecretSalt = secondHalf } };
            StorageService<SecurityConfiguration>.Service.StoreObject(securityConfig, ConfigurationGlobals.SecurityFileName);
            return Ok();
        }

        private RunResult List()
        {
            if (Configuration.Secret.Secrets == null) return Ok();
            foreach (var secret in Configuration.Secret.Secrets) ConsoleService.Service.WriteObjectDescription($"{GetType().Name}", secret.Name, $"{string.Join(',', secret.Options.Keys)}");
            return Ok();
        }
        private RunResult Get()
        {
            var name = Input.SingleQuote;
            var secret = Configuration.Secret.Secrets.FirstOrDefault(s => s.Name.ToLower() == name.ToLower());
            if (secret == null) return BadParameterError($"No secret with name \"{name}\" found.");

            var val = SecretService.Service.GetSecret(name, secret.Options, EncryptionService.Service.DecryptString);
            ConsoleService.Service.WriteObjectDescription($"{GetType().Name}", name, val);

            return Ok();
        }
        private RunResult Create()
        {
            var name = Input.SingleQuote;
            Console.Write("Enter secret: ");
            var password = PasswordPromptService.Service.ReadPassword();
            Console.WriteLine();
            Console.Write("Confirm secret: ");
            var passwordConfirm = PasswordPromptService.Service.ReadPassword();

            if (password != passwordConfirm)
            {
                Console.WriteLine();
                return BadParameterError("Passwords do not match");
            }
            var secret = new SecretItemConfiguration { Name = name };
            var val = SecretService.Service.SetSecret(name, password, secret.Options, EncryptionService.Service.EncryptString);

            Configuration.Secret ??= new();
            Configuration.Secret.Secrets ??= new();
            Configuration.Secret.Secrets.Add(secret);
            ConfigurationService.Service.SaveChanges(Configuration);
            Console.WriteLine();
            WriteHeadLine("New secret created and stored in configuration file");
            ConsoleService.Service.WriteObjectDescription($"{GetType().Name}", name, val);

            return Ok();
        }
        private RunResult Remove()
        {
            var name = Input.SingleQuote;

            var secret = Configuration.Secret.Secrets.FirstOrDefault(s => s.Name.ToLower() == name.ToLower());
            if (secret == null) return BadParameterError($"No secret with name \"{name}\" found.");

            Configuration.Secret.Secrets.Remove(secret);
            ConfigurationService.Service.SaveChanges(Configuration);

            WriteHeadLine("Secret removed from configuration file\nManually remove the secret key from environment variables or vault depending on how they are stored.");
            return Ok();
        }
    }
}