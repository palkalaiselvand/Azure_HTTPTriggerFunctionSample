using SampleApp.Shared.Enums;
using SampleApp.Shared.ProcessEntities;
using System;

namespace SampleApp.Shared.Extension
{
    public static class AzureAssets
    {
        public static AzureMessageContext GetAzureAssets(string config)
        {
            /*Sample config: "StorageAccount|Queue" */

            AzureMessageContext response = new AzureMessageContext();
            string asstype = string.Empty, queuetype = string.Empty;
            if (!string.IsNullOrWhiteSpace(config))
            {
                string[] configArray = config?.Split('|');
                if (configArray?.Length > 0)
                    asstype = configArray[0];
                if (configArray.Length > 1)
                    queuetype = configArray[1];

            }

            switch (asstype) // determine the type of service to use based on configuratino
            {
                case "ServiceBus":
                    response.AssetsType = AssetsType.ServiceBus;
                    response.ConnectionString = Environment.GetEnvironmentVariable(AppSettingsKey.SampleApp_ServiceBus_ConnectionString);
                    break;
                case "StorageAccount":
                default:
                    response.AssetsType = AssetsType.StorageAccount;
                    response.ConnectionString = Environment.GetEnvironmentVariable(AppSettingsKey.SampleApp_StorageAccount_ConnectionString);
                    break;
            }
            switch (queuetype) // // determine the type of queue service to use based on configuration
            {
                case "Topic":
                    response.QueueType = response.AssetsType == AssetsType.ServiceBus ? QueueType.Topic : QueueType.Queue;
                    response.QueueOrTopicName = response.AssetsType == AssetsType.ServiceBus ?
                        Environment.GetEnvironmentVariable(AppSettingsKey.SampleApp_ServiceBus_TopicName)
                        :
                        Environment.GetEnvironmentVariable(AppSettingsKey.SampleApp_StorageQueueName);
                    break;
                case "Queue":
                default:
                    response.QueueType = QueueType.Queue;
                    response.QueueOrTopicName = response.AssetsType == AssetsType.ServiceBus ?
                        Environment.GetEnvironmentVariable(AppSettingsKey.SampleApp_ServiceBus_QueueName)
                        :
                        Environment.GetEnvironmentVariable(AppSettingsKey.SampleApp_StorageQueueName);
                    break;
            }

            return response;
        }
    }
}
