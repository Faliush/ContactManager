using Faliush.ContactManager.Models.Base;

namespace Faliush.ContactManager.Core.Logic.PersonLogic.ViewModels;

public class PersonUpdateViewModel
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    public GenderOptions Gender { get; set; }
    public string? Address { get; set; }
    public Guid? CountryId { get; set; }
}
