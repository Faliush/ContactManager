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
    public string Gender { get; set; } = null!;

    [MaxLength(80)]
    public string? Address { get; set; }
    public Guid? CountryId { get; set; }
}
