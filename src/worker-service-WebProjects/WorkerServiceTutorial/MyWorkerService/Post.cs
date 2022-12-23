namespace WorkerServiceTutorial.WebApi.Data;

public class Post
{
    public Guid PostID { get; set; }
    public string Caption { get; set; } = "";
    public string MainBody { get; set; } = "";
    public DateTime Created { get; set; }
}