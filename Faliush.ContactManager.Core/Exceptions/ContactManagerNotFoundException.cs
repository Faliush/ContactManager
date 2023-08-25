namespace Faliush.ContactManager.Core.Exceptions;

public class ContactManagerNotFoundException : Exception
{
    public ContactManagerNotFoundException() : base("item not found") { }

    public ContactManagerNotFoundException(string? message) : base(message) { }

    public ContactManagerNotFoundException(string? message, Exception? exception) : base(message, exception) { }
}
