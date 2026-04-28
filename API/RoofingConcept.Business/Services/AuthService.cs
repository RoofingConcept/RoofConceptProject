using Microsoft.AspNetCore.Identity;
using RoofingConcept.Business.Dtos;
using RoofingConcept.Business.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoofingConcept.Business.Services;

public class AuthService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
{
    private readonly UserManager<IdentityUser> _userManager = userManager;
    private readonly SignInManager<IdentityUser> _signInManager = signInManager;

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

        var entity = new IdentityUser()
        {
            UserName = dto.Email,
            Email = dto.Email
        };

        var result = await _userManager.CreateAsync(entity, dto.Password);

        if (result.Succeeded)
        {
            return new AuthServiceResult
            {

            };
        }

        return new AuthServiceResult
        {

        };
    }

    public async Task<AuthServiceResult> SignInAsync(SignInDto dto)
    {
        if (dto == null)
        {
            return new AuthServiceResult
            {
                Success = false,
                Error = "Dto is null in signin"
            };
        }
        var entity = new IdentityUser()
        {
            UserName = dto.Email,
            Email = dto.Email
        };
        var result = await _signInManager.PasswordSignInAsync(entity, dto.Password, false, false);

        if (result.Succeeded)
        {
            return new AuthServiceResult
            {

            };
        }

        return new AuthServiceResult
        {

        };
    }

}
