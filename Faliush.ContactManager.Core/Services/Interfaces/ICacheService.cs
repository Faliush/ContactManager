namespace Faliush.ContactManager.Core.Services.Interfaces;

public interface ICacheService
{
    Task<T?>  GetAsync<T>(string key, CancellationToken cancellationToken = default) 
        where T : class;

    Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default)
        where T : class;

    Task RemoveAsync(string key, CancellationToken cancellationToken = default);

    Task RemoveByPrefixAsync(string prefixKey, CancellationToken cancellationToken = default);
}
