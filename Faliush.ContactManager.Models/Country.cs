using Faliush.ContactManager.Models.Base;

namespace Faliush.ContactManager.Models;

public class Country : Identity
{
    public string Name { get; set; } = null!;
    public virtual List<Person>? People { get; set; }
}
