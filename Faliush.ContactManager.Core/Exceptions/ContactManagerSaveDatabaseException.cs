namespace Faliush.ContactManager.Core.Exceptions;

public class ContactManagerSaveDatabaseException : Exception
{
    public ContactManagerSaveDatabaseException() : base("Database save error") { }

    public ContactManagerSaveDatabaseException(string? message) : base(message) { }

    public ContactManagerSaveDatabaseException(string? message, Exception? exception) : base(message, exception) { }
}
