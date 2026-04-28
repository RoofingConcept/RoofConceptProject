using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RoofingConcept.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    [HttpPost("signin")]
    public async Task<IActionResult> SignIn()
    {
        return Ok();
    }
}
