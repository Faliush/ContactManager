using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Models;

public class RegisterViewModel
{
    [Required]
    public string UserName { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Password and Confirm Password should be same")]
    public string ConfirmPassword { get; set; } = null!;

    [Required]
    public bool AgreeWithPolitics { get; set; }
}
