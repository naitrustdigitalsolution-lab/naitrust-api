namespace Naitrust.Infrastructure.Data.Interfaces;

public interface IUnitOfWork : IDisposable
{
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
    IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
    int SaveChanges();
    Task<int> SaveChangesAsync();
}
