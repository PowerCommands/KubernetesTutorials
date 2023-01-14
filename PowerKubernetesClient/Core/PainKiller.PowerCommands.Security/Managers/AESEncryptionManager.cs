using PainKiller.PowerCommands.Security.Contracts;
using System.Security.Cryptography;

namespace PainKiller.PowerCommands.Security.Managers
{
    public class AESEncryptionManager : IEncryptionManager
    {
        private readonly byte[] _saltBytes;

        public AESEncryptionManager(string salt) => _saltBytes = Convert.FromBase64String(salt);

        /// <summary>
        ///     Encrypt the given string using AES.  The string can be decrypted using
        ///     DecryptStringAES().  The sharedSecret parameters must match.
        /// </summary>
        /// <param name="plainText">The text to encrypt.</param>
        /// <param name="sharedSecret">A password used to generate a key for encryption.</param>
        public string EncryptString(string plainText, string sharedSecret)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException(nameof(plainText));
            if (string.IsNullOrEmpty(sharedSecret))
                throw new ArgumentNullException(nameof(sharedSecret));

            string outStr; // Encrypted string to return
            using var aesAlg = Aes.Create();
            try
            {
                var key = new Rfc2898DeriveBytes(sharedSecret, _saltBytes);
                aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                aesAlg.Mode = CipherMode.CBC;

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using var msEncrypt = new MemoryStream();

                msEncrypt.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
                msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using var swEncrypt = new StreamWriter(csEncrypt);
                    //Write all data to the stream.
                    swEncrypt.Write(plainText);
                }
                outStr = Convert.ToBase64String(msEncrypt.ToArray());
            }
            finally
            {
                aesAlg.Clear();
            }
            return outStr;
        }

        /// <summary>
        ///     Decrypt the given string.  Assumes the string was encrypted using
        ///     EncryptStringAES(), using an identical sharedSecret.
        /// </summary>
        /// <param name="cipherText">The text to decrypt.</param>
        /// <param name="sharedSecret">A password used to generate a key for decryption.</param>
        public string DecryptString(string cipherText, string sharedSecret)
        {
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentNullException(nameof(cipherText));
            if (string.IsNullOrEmpty(sharedSecret))
                throw new ArgumentNullException(nameof(sharedSecret));

            var aesAlg = Aes.Create();

            string plaintext;
            try
            {
                // generate the key from the shared secret and the salt
                var key = new Rfc2898DeriveBytes(sharedSecret, _saltBytes);

                var bytes = Convert.FromBase64String(cipherText);
                using var msDecrypt = new MemoryStream(bytes);
                aesAlg = Aes.Create();
                aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                aesAlg.IV = ReadByteArray(msDecrypt);
                aesAlg.Mode = CipherMode.CBC;
                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
                using var srDecrypt = new StreamReader(csDecrypt);
                plaintext = srDecrypt.ReadToEnd();
            }
            finally { aesAlg.Clear(); }
            return plaintext;
        }

        private static byte[] ReadByteArray(Stream s)
        {
            var rawLength = new byte[sizeof(int)];
            if (s.Read(rawLength, 0, rawLength.Length) != rawLength.Length) throw new SystemException("Stream did not contain properly formatted byte array");

            var buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
            if (s.Read(buffer, 0, buffer.Length) != buffer.Length) throw new SystemException("Did not read byte array properly");

            return buffer;
        }
    }
}