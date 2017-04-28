using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Denifia.Stardew.SendItemsApi.Domain
{
    public interface ITableStorageRepository
    {
        Task<bool> InsertOrReplace<TEntity>(TEntity entity) where TEntity : ITableEntity;
        Task<TEntity> Retrieve<TEntity>(string partitionKey, string rowKey) where TEntity : ITableEntity;
        Task<bool> Delete<TEntity>(TEntity entity) where TEntity : ITableEntity;
        Task<int> Count<TEntity>() where TEntity : ITableEntity, new();
    }

    public class AzureTableStorageRepository : ITableStorageRepository
    {
        private const string _tableName = "MailTable";
        CloudStorageAccount _storageAccount;
        CloudTableClient _tableClient;
        CloudTable _mailTable;
        AzureStorageConfig _azureTableStorageConfig;

        public AzureTableStorageRepository(IOptions<AzureStorageConfig> azureTableStorageConfig)
        {
            _azureTableStorageConfig = azureTableStorageConfig.Value;
            var storageCredentials = new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(_azureTableStorageConfig.AccountName, _azureTableStorageConfig.AccountKey);
            _storageAccount = new CloudStorageAccount(storageCredentials, _azureTableStorageConfig.EndpointSuffix, _azureTableStorageConfig.UseHttps);
            _tableClient = _storageAccount.CreateCloudTableClient();
            _mailTable = _tableClient.GetTableReference(_tableName);
            _mailTable.CreateIfNotExistsAsync().Wait();
        }

        public async Task<bool> InsertOrReplace<TEntity>(TEntity entity) where TEntity : ITableEntity
        {
            try
            {
                var insert = TableOperation.InsertOrReplace(entity);
                var result = await _mailTable.ExecuteAsync(insert);
                if (result.Result != null)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                
            }
            return false;
        }

        public async Task<TEntity> Retrieve<TEntity>(string partitionKey, string rowKey) where TEntity : ITableEntity
        {
            try
            {
                var retrieve = TableOperation.Retrieve<TEntity>(partitionKey, rowKey);
                var result = await _mailTable.ExecuteAsync(retrieve);
                if (result.Result != null)
                {
                    return (TEntity)result.Result;
                }
            }
            catch (Exception ex)
            {

            }
            return default(TEntity);
        }

        public async Task<bool> Delete<TEntity>(TEntity entity) where TEntity : ITableEntity
        {
            try
            {
                if (entity.ETag == null || entity.ETag.Equals(string.Empty))
                {
                    entity.ETag = "*";
                }
                var delete = TableOperation.Delete(entity);
                var result = await _mailTable.ExecuteAsync(delete);
                if (result.Result != null)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {

            }
            return false;
        }

        public async Task<int> Count<TEntity>() where TEntity : ITableEntity, new()
        {
            var columns = new List<string>() { "Id" };
            var query = new TableQuery<TEntity>().Select(columns);
            var continuationToken = (TableContinuationToken)null;
            var count = 0;
            do
            {
                var segment = await _mailTable.ExecuteQuerySegmentedAsync(query, continuationToken);
                continuationToken = segment.ContinuationToken;
                count += segment.Results.Count();
            }
            while (continuationToken != null);

            return count;
        }
    }
}
