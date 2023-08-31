namespace Faliush.ContactManager.Core.Exceptions;

public class ContactManagerArgumentException : Exception
{
    public ContactManagerArgumentException() : base("uncorrect argument") { }

    public ContactManagerArgumentException(string? message) : base(message) { }

    public ContactManagerArgumentException(string? message, Exception? exception) : base(message, exception) { }
}
