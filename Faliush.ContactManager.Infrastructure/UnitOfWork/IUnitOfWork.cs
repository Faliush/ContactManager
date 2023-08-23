using Microsoft.EntityFrameworkCore;

namespace Faliush.ContactManager.Infrastructure.UnitOfWork;

public interface IUnitOfWork<out TContext> : IUnitOfWork where TContext : DbContext
{
    /// <summary>
    /// 
    /// </summary>
    TContext DbContext { get; }
}

public interface IUnitOfWork : IDisposable 
{
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

