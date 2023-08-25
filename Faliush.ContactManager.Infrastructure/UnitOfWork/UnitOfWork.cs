using Microsoft.EntityFrameworkCore;

namespace Faliush.ContactManager.Infrastructure.UnitOfWork;

public sealed class UnitOfWork<TContext> : IUnitOfWork<TContext>, IRepositoryFactory
    where TContext : DbContext
{
    private Dictionary<Type, object> _repositories;
    private bool _disposed;

    public UnitOfWork(TContext context)
    {
        DbContext = context ?? throw new ArgumentNullException(nameof(context));
        LastSaveChangeResult = new SaveChangesResult();
    }

    public TContext DbContext { get; }

    public SaveChangesResult LastSaveChangeResult { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
    {
        _repositories ??= new Dictionary<Type, object>();

        var type = typeof(TEntity);

        if (!_repositories.ContainsKey(type))
            _repositories[type] = new Repository<TEntity>(DbContext);

        return (Repository<TEntity>)_repositories[type];
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public int SaveChanges()
    {
        try
        {
            return DbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            LastSaveChangeResult.Exception = ex;
            return 0;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task<int> SaveChangesAsync()
    {
        try
        {
            return await DbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            LastSaveChangeResult.Exception = ex;
            return 0;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="disposing"></param>
    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _repositories?.Clear();
                DbContext.Dispose();
            }
        }

        _disposed = true;
    }
}