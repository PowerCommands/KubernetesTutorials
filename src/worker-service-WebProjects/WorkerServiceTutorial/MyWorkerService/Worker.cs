namespace MyWorkerService;
public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    public Worker(ILogger<Worker> logger) => _logger = logger;
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
        var socketsHttpHandler = new SocketsHttpHandler() { PooledConnectionIdleTimeout = TimeSpan.FromMinutes(1) };
        using var httpClient = new HttpClient(socketsHttpHandler);
        var uri = $"{Environment.GetEnvironmentVariable("WebApiUrl")}/blog";
        var post = new Post { Caption = "Worker service did it again!", MainBody = "Lorum ipsum", PostID = Guid.NewGuid() };
        var request = new HttpRequestMessage(HttpMethod.Post, uri) { Content = new StringContent(JsonSerializer.Serialize(post), Encoding.UTF8, "application/json") };
        await httpClient.SendAsync(request, stoppingToken);
    }
}
