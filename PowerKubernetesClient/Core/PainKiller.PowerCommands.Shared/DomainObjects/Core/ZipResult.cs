namespace PainKiller.PowerCommands.Shared.DomainObjects.Core
{
    public class ZipResult
    {
        public int FileCount { get; set; }
        public string? Checksum { get; set; }
        public long FileSizeUncompressedInKb { get; set; }
        public long FileSizeCompressedInKb { get; set; }
        public string? Path { get; set; }
        public List<string> FileNames { get; set; } = new();
        public string ExceptionMessage { get; set; } = "";
        public bool HasException { get; set; } = false;
    }
}