using Faliush.ContactManager.Infrastructure.UnitOfWork.Pagination;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Faliush.ContactManager.Infrastructure.UnitOfWork;

public interface IRepository<TEntity> where TEntity : class
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="orderBy"></param>
    /// <param name="include"></param>
    /// <param name="disableTracking"></param>
    /// <returns></returns>
    IQueryable<TEntity> GetAll(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = false,
        bool ignoreQueryFilters = false,
        bool ignoreAutoInclude = false);

    IPagedList<TEntity> GetPagedList(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int pageIndex = 0,
        int pageSize = 20,
        bool disableTracking = false,
        bool ignoreQueryFilters = false,
        bool ignoreAutoInclude = false);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="orderBy"></param>
    /// <param name="include"></param>
    /// <param name="disableTracking"></param>
    /// <returns></returns>
    Task<IList<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = false,
        bool ignoreQueryFilters = false,
        bool ignoreAutoInclude = false);

    Task<IPagedList<TEntity>> GetPagedListAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int pageIndex = 0,
        int pageSize = 20,
        bool disableTracking = false,
        bool ignoreQueryFilters = false,
        bool ignoreAutoInclude = false);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="orderBy"></param>
    /// <param name="include"></param>
    /// <param name="disableTracking"></param>
    /// <returns></returns>
    TEntity? GetFirstOrDefault(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = false,
        bool ignoreQueryFilters = false,
        bool ignoreAutoInclude = false);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="orderBy"></param>
    /// <param name="include"></param>
    /// <param name="disableTracking"></param>
    /// <returns></returns>
    Task<TEntity?> GetFirstOrDefaultAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = false,
        bool ignoreQueryFilters = false,
        bool ignoreAutoInclude = false);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="entity"></param>
    void Insert(TEntity entity);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    ValueTask<EntityEntry<TEntity>> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="entity"></param>
    void Update(TEntity entity);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    void Delete(object id);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="entity"></param>
    void Delete(TEntity entity);


}