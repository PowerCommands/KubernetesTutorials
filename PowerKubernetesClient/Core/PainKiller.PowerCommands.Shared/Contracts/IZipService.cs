using PainKiller.PowerCommands.Shared.DomainObjects.Core;

namespace PainKiller.PowerCommands.Shared.Contracts;

public interface IZipService
{
    ZipResult ArchiveFilesInDirectory(string directoryPath, string archiveName, bool useTimestampSuffix = false, string filter = "*", string outputDirectory = "");
}