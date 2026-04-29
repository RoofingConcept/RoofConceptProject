using Microsoft.AspNetCore.Identity;
using RoofingConcept.Business.Dtos;
using RoofingConcept.Business.Results;
using RoofingConcept.Data.Entities;

namespace RoofingConcept.Business.Services;

public interface IAuthService
{
    Task<AuthServiceResult> SignInAsync(SignInDto dto);
    Task<AuthServiceResult> SignOutAsync();
    Task<AuthServiceResult> SignUpAsync(SignUpDto dto);
}

public class AuthService(UserManager<ApplicationUserEntity> userManager, SignInManager<ApplicationUserEntity> signInManager) : IAuthService
{

    private readonly UserManager<ApplicationUserEntity> _userManager = userManager;
    private readonly SignInManager<ApplicationUserEntity> _signInManager = signInManager;

    public async Task<AuthServiceResult> SignUpAsync(SignUpDto dto)
    {
        if (dto == null)
        {
            return new AuthServiceResult
            {
                Success = false,
                Error = "Dto is null in signup"
            };
        }

        var entity = new ApplicationUserEntity
        {
            UserName = dto.Email,
            Email = dto.Email
        };

        var result = await _userManager.CreateAsync(entity, dto.Password);

        if (!result.Succeeded)
        {
            return new AuthServiceResult
            {
                Success = false,
                Error = "Unable to create account with the provided details."
            };
        }

        return new AuthServiceResult
        {
            Success = true,
            Message = "User created"
        };
    }

    public async Task<AuthServiceResult> SignInAsync(SignInDto dto)
    {
        if (dto == null)
        {
            return new AuthServiceResult
            {
                Success = false,
                Error = "Invalid login attempt"
            };
        }

        var entity = await _userManager.FindByEmailAsync(dto.Email);
        if (entity == null)
        {
            return new AuthServiceResult
            {
                Success = false,
                Error = "Invalid email or password"
            };
        }

        var result = await _signInManager.PasswordSignInAsync(entity, dto.Password, false, true);
        if (result.IsLockedOut)
        {
            return new AuthServiceResult
            {
                Success = false,
                Error = "Invalid email or password"
            };
        }

        return result.Succeeded
           ? new AuthServiceResult
           {
               Success = true,
               Message = "User signed in"
           }
           : new AuthServiceResult
           {
               Success = false,
               Error = "Invalid email or password"
           };
    }

    public async Task<AuthServiceResult> SignOutAsync()
    {
        await _signInManager.SignOutAsync();

        return new AuthServiceResult
        {
            Success = true,
            Message = "Signed out successfully."
        };
    }
}
