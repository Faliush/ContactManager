namespace Faliush.ContactManager.Core.Services;

public interface IStringConvertService
{
    public T ConvertToEnum<T>(string value) where T : struct;
}
