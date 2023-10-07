using BlazorClient.Enums;
using System.ComponentModel.DataAnnotations;

namespace BlazorClient.DTO.Results;

public class UpdatePersonResult
{
    public Guid Id { get; set; }

    [Required]
    [MinLength(2)]
    [MaxLength(30)]
    public string FirstName { get; set; } = null!;

    [Required]
    [MinLength(2)]
    [MaxLength(30)]
    public string LastName { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [Phone]
    public string Phone { get; set; } = null!;

    [Required]
    public DateTime DateOfBirth { get; set; }

    [Required]
    public GenderOptions Gender { get; set; }

    [MaxLength(80)]
    public string? Address { get; set; }
    
    [Required]
    public Guid? CountryId { get; set; }
}
