namespace Faliush.ContactManager.Models.Base;

public abstract class Identity : IHaveId
{
    public Guid Id { get; set; }
}
