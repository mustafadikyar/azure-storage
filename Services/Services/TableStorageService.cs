using Microsoft.Azure.Cosmos.Table;
using Services.Config;
using Services.Interfaces;
using System.Linq.Expressions;

namespace Services.Services
{
    public class TableStorageService<TEntity> : ITableStorageService<TEntity> where TEntity : TableEntity, new()
    {
        private readonly CloudTableClient _client;
        private readonly CloudTable _table;

        public TableStorageService()
        {
            var account = CloudStorageAccount.Parse(AppSettings.AzureStorageConnectionString);
            _client = account.CreateCloudTableClient();
            _table = _client.GetTableReference(typeof(TEntity).Name);
            _table.CreateIfNotExists();
        }

        public async Task<TEntity> Add(TEntity entity)
        {
            var operation = TableOperation.InsertOrMerge(entity);
            var execute = await _table.ExecuteAsync(operation);
            return execute.Result as TEntity;
        }

        public async Task Delete(string rowKey, string partitionKey)
        {
            var entity = await GetSingle(rowKey, partitionKey);
            var operation = TableOperation.Delete(entity);
            await _table.ExecuteAsync(operation);
        }

        public IQueryable<TEntity> GetAll()
        {
            return _table.CreateQuery<TEntity>().AsQueryable();
        }

        public async Task<TEntity> GetSingle(string rowKey, string partitionKey)
        {
            var operation = TableOperation.Retrieve<TEntity>(partitionKey, rowKey);
            var execute = await _table.ExecuteAsync(operation);
            return execute.Result as TEntity;
        }

        public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> query)
        {
            return _table.CreateQuery<TEntity>().Where(query);
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            var operation = TableOperation.Replace(entity);
            var execute = await _table.ExecuteAsync(operation);
            return execute.Result as TEntity;
        }
    }
}
