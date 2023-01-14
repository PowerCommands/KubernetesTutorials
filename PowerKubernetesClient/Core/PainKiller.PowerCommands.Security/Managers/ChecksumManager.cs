using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace PainKiller.PowerCommands.Security.Managers
{
    internal static class ChecksumManager
    {
        internal static string CalculateMd5ForFile(string filename)
        {
            if (!File.Exists(filename)) return "file missing...";
            using var md5 = MD5.Create();
            using var stream = File.OpenRead(filename);
            var hash = md5.ComputeHash(stream);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }
        internal static string CalculateMd5<TItem>(TItem content)
        {
            var options = new JsonSerializerOptions { WriteIndented = true, IncludeFields = true };
            var jsonString = JsonSerializer.Serialize(content, options);
            using var md5 = MD5.Create();
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString ?? ""));
            var hash = md5.ComputeHash(stream);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }
    }
}