using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Parkway.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AuthTestController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var user = HttpContext.User;
        return Ok(new
        {
            Message = "You are authenticated 🎉",
            Name = user.Identity?.Name,
            Claims = user.Claims.Select(c => new { c.Type, c.Value })
        });
    }
}
