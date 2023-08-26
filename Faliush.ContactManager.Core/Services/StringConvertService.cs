using Faliush.ContactManager.Core.Exceptions;
using Faliush.ContactManager.Models.Base;
using System.Net.Http.Headers;

namespace Faliush.ContactManager.Core.Services;

public class StringConvertService : IStringConvertService
{
    public T ConvertToEnum<T>(string value) where T : struct
    {
        if(Enum.TryParse<T>(value, out T result))
            return result;

        throw new ContactManagerInvalidOperationException("cannot convert string to enum");
       
    }
}
