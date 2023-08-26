namespace Faliush.ContactManager.Core.Exceptions;
public class ContactManagerInvalidOperationException : Exception
{
    public ContactManagerInvalidOperationException() : base("something went wrong") { }

    public ContactManagerInvalidOperationException(string? message) : base(message) { }

    public ContactManagerInvalidOperationException(string? message, Exception? exception) : base(message, exception) { }
}
