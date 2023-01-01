namespace WorkerServiceTutorial.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DiagnosticController : ControllerBase
{
    [HttpGet]
    public ActionResult<string> Get() => Ok("API version 1.0 is up and running...");
}