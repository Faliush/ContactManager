using Faliush.ContactManager.Core.Logic.CountryLogic.ViewModels;
using Faliush.ContactManager.Models.Base;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Faliush.ContactManager.Core.Logic.PersonLogic.ViewModels;

public class PersonCreateViewModel
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; } = null!;
    public string? Address { get; set; }
    public Guid? CountryId { get; set; }

    [BindNever] // we will send it to UI but we don't recive it 
    public List<CountryViewModel> Countries { get; set; } = null!;

    [BindNever]
    public List<string> Genders { get; set; } = null!;
}
