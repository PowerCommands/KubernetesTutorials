using PainKiller.PowerCommands.Security.DomainObjects;

namespace PainKiller.PowerCommands.Security.Extensions;

public static class SecurityExtensions
{
    public static bool CompareFileChecksum(this FileChecksum fileChecksum, string checksum) => fileChecksum.Mde5Hash == checksum;
}