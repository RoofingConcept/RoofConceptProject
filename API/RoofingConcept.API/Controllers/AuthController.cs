using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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
    public async Task<IActionResult> SignIn(SignUpViewModel signUpVM)
    {
        if (signUpVM == null)
        {
            return NoContent();
        }

        var dto = new SignUpDto
        {
            Email = signUpVM.Email,
            Password = signUpVM.Password,
        };

        var result = await _authService.SignUpAsync(dto);

        return result.Success 
            ? Ok(result) 
            : BadRequest();
    }

    [HttpPost("signin")]
    public async Task<IActionResult> SignIn(SignInViewModel signInVM)
    {
        var dto = new SignInDto
        {
            Email = signInVM.Email,
            Password = signInVM.Password
        };

        var result = await _authService.SignInAsync(dto);

        return result.Success
            ? Ok(result)
            : BadRequest();
    }

    [HttpPost("signout")]
    public async Task<IActionResult> SignOut()
    {
        var result = await _authService.SignOutAsync();

            return result.Success
            ? Ok(result)
            : BadRequest();
    }
}
