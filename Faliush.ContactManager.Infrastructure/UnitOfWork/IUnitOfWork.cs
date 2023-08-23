namespace Faliush.ContactManager.Infrastructure.UnitOfWork;

public interface IUnitOfWork<out TContext> : IDisposable
{
    /// <summary>
    /// 
    /// </summary>
    TContext DbContext { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    int SaveChanges();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<int> SaveChangesAsync();

    SaveChangesResult LastSaveChangeResult { get; }
}
