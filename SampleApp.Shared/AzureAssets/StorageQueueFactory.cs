using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Queue;
using Newtonsoft.Json;
using Polly;
using SampleApp.Shared.AzureAssets.Abstraction;
using System;
using System.Threading.Tasks;

namespace SampleApp.Shared.AzureAssets
{
    public class StorageQueueFactory : IAzureStorageFactory
    {
        private readonly CloudStorageAccount _storageAccount;
        private readonly CloudQueueClient _cloudQueueClient;

        public StorageQueueFactory()
        {
            _storageAccount = CloudStorageAccount.Parse(Environment.GetEnvironmentVariable(AppSettingsKey.SampleApp_StorageAccount_ConnectionString));
            _cloudQueueClient = _storageAccount.CreateCloudQueueClient();
        }
        public async Task SendMessage<T>(T message, string queueName)
        {
            try
            {
                var hasMessageSent = await AddMessageAsync<T>(queueName, message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<bool> AddMessageAsync<T>(string queue, T message)
        {
            var cloudQueue = _cloudQueueClient.GetQueueReference(queue.ToLower());
            await Policy
                .Handle<StorageException>()
                .RetryAsync(1, async (exception, retryCount) =>
                {
                    await cloudQueue.CreateIfNotExistsAsync();
                })
                .ExecuteAsync(async () =>
                {
                    await cloudQueue.AddMessageAsync(new CloudQueueMessage(JsonConvert.SerializeObject(message)));
                });
            return true;
        }
    }
}
