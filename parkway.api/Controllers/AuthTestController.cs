using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Parkway.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthTestController : ControllerBase
{
    [Authorize]
    [HttpGet("ping")]
    public IActionResult Ping() => Ok("🟢 Authenticated and authorized.");
}
