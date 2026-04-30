using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoofingConcept.API.ViewModels;
using RoofingConcept.Business.Dtos;
using RoofingConcept.Business.Services;
using System.Security.Claims;

namespace RoofingConcept.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    [HttpPost("signup")]
    public async Task<IActionResult> SignUp(SignUpViewModel signUpVM)
    {
        if (signUpVM == null)
        {
            return BadRequest(new { error = "Request body is required." });
        }

        var dto = new SignUpDto
        {
            Email = signUpVM.Email,
            Password = signUpVM.Password,
        };

        var result = await _authService.SignUpAsync(dto);

        return result.Success
            ? Ok(result)
            : BadRequest(result);
    }

    [HttpPost("signin")]
    public async Task<IActionResult> SignIn(SignInViewModel signInVM)
    {
        if (signInVM == null)
        {
            return BadRequest(new { error = "Request body is required." });
        }

        var dto = new SignInDto
        {
            Email = signInVM.Email,
            Password = signInVM.Password
        };

        var result = await _authService.SignInAsync(dto);

        return result.Result?.Success == true
            ? Ok(result)
            : Unauthorized(result);
    }

    [Authorize]
    [HttpPost("signout")]
    public async Task<IActionResult> SignOutUser()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _authService.SignOutAsync(userId);

        return result.Success
            ? Ok(result)
            : BadRequest(result);
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _authService.GetCurrentUserAsync(userId);

        return user == null
            ? Unauthorized()
            : Ok(user);
    }
}
