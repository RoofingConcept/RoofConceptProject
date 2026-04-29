using Microsoft.AspNetCore.Mvc;
using RoofingConcept.API.ViewModels;
using RoofingConcept.Business.Dtos;
using RoofingConcept.Business.Services;

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

        return result.Success
            ? Ok(result)
            : BadRequest(result);
    }

    [HttpPost("signout")]
    public async Task<IActionResult> SignOutUser()
    {
        var result = await _authService.SignOutAsync();

        return result.Success
            ? Ok(result)
            : BadRequest(result);
    }
}
