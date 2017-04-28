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
        Task InsertOrReplace<TEntity>(TEntity entity) where TEntity : ITableEntity;
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

        public async Task InsertOrReplace<TEntity>(TEntity entity) where TEntity : ITableEntity
        {
            var insert = TableOperation.InsertOrReplace(entity);
            await _mailTable.ExecuteAsync(insert);
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
