using Faliush.ContactManager.Models.Base;

namespace Faliush.ContactManager.Models;

public class Person : Auditable
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    public GenderOptions Gender { get; set; }
    public string? Address { get; set; }

    public Guid? CountryId { get; set; }
    public Country? Country { get; set; }
}
