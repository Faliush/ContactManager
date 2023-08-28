using Faliush.ContactManager.Core.Exceptions;

namespace Faliush.ContactManager.Core.Services;

public class StringConvertService : IStringConvertService
{
    public T ConvertToEnum<T>(string value) where T : struct
    {
        if(Enum.TryParse<T>(value, out T result))
            return result;

        throw new ContactManagerInvalidOperationException("given string doesn't exist in enum");
    }
}
