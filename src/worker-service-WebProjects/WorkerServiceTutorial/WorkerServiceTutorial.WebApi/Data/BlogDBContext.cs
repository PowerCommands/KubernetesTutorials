namespace WorkerServiceTutorial.WebApi.Data;
public sealed class BlogDBContext : DbContext
{
    public BlogDBContext(DbContextOptions<BlogDBContext> options) : base(options) => Database.EnsureCreated();
    public DbSet<Post> Posts { get; set; } = null!;
}