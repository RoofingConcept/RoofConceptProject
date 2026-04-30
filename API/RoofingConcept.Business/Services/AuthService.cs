using Microsoft.AspNetCore.Identity;
using RoofingConcept.Business.Dtos;
using RoofingConcept.Business.Results;
using RoofingConcept.Data.Entities;

namespace RoofingConcept.Business.Services;

public interface IAuthService
{
    Task<CurrentUserDto?> GetCurrentUserAsync(string? userId);
    Task<AuthServiceResult<JwtServiceResult>> SignInAsync(SignInDto dto);
    Task<AuthServiceResult> SignOutAsync(string? userId);
    Task<AuthServiceResult> SignUpAsync(SignUpDto dto);
}

public class AuthService(UserManager<ApplicationUserEntity> userManager, SignInManager<ApplicationUserEntity> signInManager, IJwtTokenBuilder jwtTokenBuilder) : IAuthService
{

    private readonly UserManager<ApplicationUserEntity> _userManager = userManager;
    private readonly SignInManager<ApplicationUserEntity> _signInManager = signInManager;
    private readonly IJwtTokenBuilder _jwtTokenBuilder = jwtTokenBuilder;

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

    public async Task<AuthServiceResult<JwtServiceResult>> SignInAsync(SignInDto dto)
    {
        if (dto == null)
        {
            return new AuthServiceResult<JwtServiceResult>
            {
                Result = new JwtServiceResult
                {
                    Success = false,
                    Error = "Invalid login attempt"
                }
            };
        }

        var entity = await _userManager.FindByEmailAsync(dto.Email);
        if (entity == null)
        {
            return new AuthServiceResult<JwtServiceResult>
            {
                Result = new JwtServiceResult
                {
                    Success = false,
                    Error = "Invalid email or password"
                }
            };
        }

        var result = await _signInManager.CheckPasswordSignInAsync(entity, dto.Password, true);
        if (result.IsLockedOut)
        {
            return new AuthServiceResult<JwtServiceResult>  
            {
                Result = new JwtServiceResult
                {
                    Success = false,
                    Error = "Invalid email or password"
                }
            };
        }

        if (!result.Succeeded)
        {
            return new AuthServiceResult<JwtServiceResult>
            {
                Result = new JwtServiceResult
                {
                    Success = false,
                    Error = "Invalid email or password"
                }
            };
        }
         
        var jwtToken = _jwtTokenBuilder.BuildJwtToken(entity);

        return new AuthServiceResult<JwtServiceResult>
        {
            Result = new JwtServiceResult
            {
                Success = true,
                Message = "User signed in",
                Token = jwtToken.Token,
                ExpiresAtUtc = jwtToken.ExpiresAtUtc
            }
        };
    }

    public async Task<AuthServiceResult> SignOutAsync(string? userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            return new AuthServiceResult
            {
                Success = false,
                Error = "Invalid user."
            };
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return new AuthServiceResult
            {
                Success = false,
                Error = "User not found."
            };
        }

        await _userManager.UpdateSecurityStampAsync(user);
        await _signInManager.SignOutAsync();

        return new AuthServiceResult
        {
            Success = true,
            Message = "Signed out successfully."
        };
    }

    public async Task<CurrentUserDto?> GetCurrentUserAsync(string? userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            return null;
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return null;
        }

        return new CurrentUserDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email
        };
    }
}
