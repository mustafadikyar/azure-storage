using System.Linq.Expressions;

namespace Services.Interfaces
{
    public interface ITableStorageService<TEntity>
    {
        Task<TEntity> GetSingle(string rowKey, string partitionKey);
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> query);
        Task<TEntity> Add(TEntity entity);
        Task<TEntity> Update(TEntity entity);
        Task Delete(string rowKey, string partitionKey);
    }
}
