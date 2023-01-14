using PainKiller.PowerCommands.Security.Services;

namespace PainKiller.PowerCommands.Core.Extensions
{
    public static class SecretExtensions
    {
        public static T DecryptSecret<T>(this SecretConfiguration secretConfiguration, T configurationItem, string propertyName) where T : class, new()
        {
            var retVal = configurationItem.DeepClone();
            var encryptedContent = (string)configurationItem.GetPropertyValue(propertyName);

            var decryptedContent = encryptedContent;
            foreach (var secret in secretConfiguration.Secrets)
            {
                var findAndReplaceContent = SecretService.Service.ReplaceSecret(encryptedContent, secret.Name, secret.Options, EncryptionService.Service.DecryptString);
                if (!string.Equals(findAndReplaceContent, decryptedContent, StringComparison.Ordinal))
                {
                    decryptedContent = findAndReplaceContent;
                    break;
                }
            }
            retVal.SetPropertyValue(propertyName, decryptedContent);
            return retVal;
        }
        public static string DecryptSecret(this SecretConfiguration secretConfiguration, string encryptedContent)
        {
            var decryptedContent = encryptedContent;
            foreach (var secret in secretConfiguration.Secrets)
            {
                var findAndReplaceContent = SecretService.Service.ReplaceSecret(encryptedContent, secret.Name, secret.Options, EncryptionService.Service.DecryptString);
                if (!string.Equals(findAndReplaceContent, decryptedContent, StringComparison.Ordinal))
                {
                    decryptedContent = findAndReplaceContent;
                    break;
                }
            }
            return decryptedContent;
        }
    }
}