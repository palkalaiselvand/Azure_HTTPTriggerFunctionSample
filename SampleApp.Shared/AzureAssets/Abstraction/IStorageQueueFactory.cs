using SampleApp.Shared.ProcessEntities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SampleApp.Shared.AzureAssets.Abstraction
{
    public interface IStorageQueueFactory
    {
        Task SendMessage<T>(T message, AzureMessageContext context);
        Task SendMessages<T>(List<T> messages, AzureMessageContext context);
    }
}
