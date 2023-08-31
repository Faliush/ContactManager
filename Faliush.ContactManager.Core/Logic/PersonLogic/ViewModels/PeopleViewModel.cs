namespace Faliush.ContactManager.Core.Logic.PersonLogic.ViewModels;

public class PeopleViewModel
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
}
