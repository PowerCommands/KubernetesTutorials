namespace WorkerServiceTutorial.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
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
    [HttpPost]
    public async Task<ActionResult<int>> Create(Post post)
    {
        await _context.Posts.AddAsync(post);
        var rowsAffected = await _context.SaveChangesAsync();
        return Ok(rowsAffected);
    }
}