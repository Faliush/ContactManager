namespace Faliush.ContactManager.Infrastructure.UnitOfWork;

public interface IRepositoryFactory
{
    IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
}
