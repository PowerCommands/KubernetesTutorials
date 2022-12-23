namespace WorkerServiceTutorial.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class BlogController : ControllerBase
{
    private readonly ILogger<BlogController> _logger;
    private readonly BlogDBContext _context;

    public BlogController(ILogger<BlogController> logger, BlogDBContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Post>>> Get()
    {
        var result = await _context.Posts.AllAsync(p => p.Created > new DateTime(2000, 1, 1));
        return Ok(result);
    }
}