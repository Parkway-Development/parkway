using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Parkway.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthTestController : ControllerBase
{
    [Authorize(Roles = "GlobalAdmin")]
    [HttpGet("claims")]
    [Authorize]
    public IActionResult GetClaims()
    {
        var claims = User.Claims.Select(c => new { c.Type, c.Value });
        return Ok(claims);
    }
}

