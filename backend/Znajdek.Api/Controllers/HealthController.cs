using Microsoft.AspNetCore.Mvc;

namespace Znajdek.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            status = "ok",
            application = "Znajdek"
        });
    }
}