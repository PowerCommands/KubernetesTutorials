namespace PainKiller.PowerCommands.Core.Services;
public class DownloadWithProgressService : IDownloadWithProgressService
{
    private static readonly Lazy<DownloadWithProgressService> Lazy = new(() => new DownloadWithProgressService());
    public static DownloadWithProgressService Service => Lazy.Value;
    public async Task Download(string downloadUrl, string destinationFilePath, Func<long?, long, double?, string, string, bool> progressChanged)
    {
        using var httpClient = new HttpClient { Timeout = TimeSpan.FromDays(1) };
        using var response = await httpClient.GetAsync(downloadUrl, HttpCompletionOption.ResponseHeadersRead);

        response.EnsureSuccessStatusCode();
        var totalBytes = response.Content.Headers.ContentLength;

        await using var contentStream = await response.Content.ReadAsStreamAsync();
        var totalBytesRead = 0L;
        var readCount = 0L;
        var buffer = new byte[8192];
        var isMoreToRead = true;

        static double? CalculatePercentage(long? totalDownloadSize, long totalBytesRead) => totalDownloadSize.HasValue ? Math.Round((double)totalBytesRead / totalDownloadSize.Value * 100, 2) : null;

        await using var fileStream = new FileStream(destinationFilePath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true);
        do
        {
            var bytesRead = await contentStream.ReadAsync(buffer);
            if (bytesRead == 0)
            {
                isMoreToRead = false;

                if (progressChanged(totalBytes, totalBytesRead, CalculatePercentage(totalBytes, totalBytesRead), downloadUrl, destinationFilePath))
                {
                    throw new OperationCanceledException();
                }

                continue;
            }
            await fileStream.WriteAsync(buffer.AsMemory(0, bytesRead));

            totalBytesRead += bytesRead;
            readCount++;

            if (readCount % 100 == 0)
            {
                if (progressChanged(totalBytes, totalBytesRead, CalculatePercentage(totalBytes, totalBytesRead), downloadUrl, destinationFilePath))
                {
                    throw new OperationCanceledException();
                }
            }
        }
        while (isMoreToRead);
    }
}