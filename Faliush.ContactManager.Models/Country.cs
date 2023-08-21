using Faliush.ContactManager.Models.Base;

namespace Faliush.ContactManager.Models;

public class Country : Identity
{
    public string Name { get; set; } = null!;
    public List<Person>? People { get; set; }
}
