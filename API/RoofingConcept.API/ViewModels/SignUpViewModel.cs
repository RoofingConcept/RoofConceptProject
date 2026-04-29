using System.ComponentModel.DataAnnotations;

namespace RoofingConcept.API.ViewModels;

public class SignUpViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = null!;
}
