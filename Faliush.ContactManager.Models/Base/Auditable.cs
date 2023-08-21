namespace Faliush.ContactManager.Models.Base;

public abstract class Auditable : Identity, IAudetable 
{
    public string CreatedBy { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
}
