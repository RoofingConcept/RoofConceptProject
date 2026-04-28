using System;
using System.Collections.Generic;
using System.Text;

namespace RoofingConcept.Business.Dtos;

public class SignInDto
{
    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;
}
