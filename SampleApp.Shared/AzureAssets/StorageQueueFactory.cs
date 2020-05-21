using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Queue;
using Newtonsoft.Json;
using Polly;
using SampleApp.Shared.AzureAssets.Abstraction;
using SampleApp.Shared.ProcessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApp.Shared.AzureAssets
{
    public class StorageQueueFactory : IStorageQueueFactory
    {
        private CloudStorageAccount _storageAccount;
        private CloudQueueClient _cloudQueueClient;

        public async Task SendMessage<T>(T message, AzureMessageContext context)
        {
            try
            {
                Initialize(context);
                var hasMessageSent = await AddMessageAsync<T>(context.QueueOrTopicName, message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task SendMessages<T>(List<T> messages, AzureMessageContext context)
        {
            try
            {
                Initialize(context);
                List<Task<bool>> postMessageTask = new List<Task<bool>>();
                foreach (var message in messages)
                {
                    postMessageTask.Add(AddMessageAsync<T>(context.QueueOrTopicName, message));
                }

                await Task.WhenAll(postMessageTask);
                bool isSuccess = postMessageTask.All(r => r.Result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<bool> AddMessageAsync<T>(string queue, T queueMessage)
        {
            var type = queueMessage.GetType();
            string data = type.Name == "String" ? queueMessage.ToString() : JsonConvert.SerializeObject(queueMessage);
            var cloudQueue = _cloudQueueClient.GetQueueReference(queue.ToLower());
            await Policy
                .Handle<StorageException>()
                .RetryAsync(1, async (exception, retryCount) =>
                {
                    await cloudQueue.CreateIfNotExistsAsync();
                })
                .ExecuteAsync(async () =>
                {
                    await cloudQueue.AddMessageAsync(new CloudQueueMessage(data));
                });
            return true;
        }

        private void Initialize(AzureMessageContext context)
        {
            _storageAccount = CloudStorageAccount.Parse(context.ConnectionString);
            _cloudQueueClient = _storageAccount.CreateCloudQueueClient();
        }
    }
}
