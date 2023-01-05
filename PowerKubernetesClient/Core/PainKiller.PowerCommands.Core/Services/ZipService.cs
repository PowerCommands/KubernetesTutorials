using System.IO.Compression;

namespace PainKiller.PowerCommands.Core.Services;
public class ZipService : IZipService
{
    private ZipService() { }
    private static readonly Lazy<IZipService> Lazy = new(() => new ZipService());
    public static IZipService Service => Lazy.Value;

    public ZipResult ArchiveFilesInDirectory(string directoryPath, string archiveName, bool useTimestampSuffix = false, string filter = "*", string outputDirectory = "")
    {
        if (!Directory.Exists(directoryPath)) return new ZipResult {ExceptionMessage = $"Could not find directory: {directoryPath}", HasException = true};
        var result = new ZipResult();
        long totalByteSize = 0;

        archiveName = useTimestampSuffix ? archiveName.FormatFileTimestamp() : archiveName;
        var tempDirectory = $"{Path.GetTempPath()}\\{archiveName}";

        var fileNames = Directory.GetFiles(directoryPath, filter);
        Directory.CreateDirectory(tempDirectory);
        foreach (var fileName in fileNames)
        {
            result.FileCount++;
            var fileInfo = new FileInfo(fileName);
            result.FileNames.Add(fileName);
            totalByteSize += fileInfo.Length;
            File.Copy(fileName, $"{tempDirectory}\\{fileInfo.Name}");
        }

        var archiveFileName = $"{archiveName}.zip";
        ZipFile.CreateFromDirectory(tempDirectory, archiveFileName);
        Directory.Delete(tempDirectory, recursive: true);
        
        var zipFileInfo = new FileInfo(archiveFileName);
        result.FileSizeUncompressedInKb = totalByteSize / 1024;
        result.FileSizeCompressedInKb =  zipFileInfo.Length / 1024;
        result.Checksum = new FileChecksum(zipFileInfo.FullName).Mde5Hash;
        result.Path =zipFileInfo.FullName;
        return result;
    }
}