using System.ComponentModel.DataAnnotations;

namespace RoofingConcept.API.ViewModels;

public class SignInViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;
}
