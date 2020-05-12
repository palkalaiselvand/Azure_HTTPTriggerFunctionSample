using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using SampleApp.Shared.AzureAssets.Abstraction;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SampleApp.Shared.AzureAssets
{
    public class ServiceBusFactory : IAzureStorageFactory
    {
        public string ServiceBusConnectionString = Environment.GetEnvironmentVariable(AppSettingsKey.SampleApp_ServiceBus_ConnectionString);
        static IQueueClient queueClient;
        public async Task SendMessage<T>(T message, string queueName)
        {
            try
            {
                queueClient = new QueueClient(ServiceBusConnectionString, queueName);

                var data = FormatMessage(message);

                queueClient.SendAsync(data).GetAwaiter().GetResult();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Message FormatMessage<T>(T queueMessage)
        {
            Message message = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(queueMessage)))
            {
                MessageId = Guid.NewGuid().ToString(),
                SessionId = Guid.NewGuid().ToString()
            };

            return message;
        }
    }
}
