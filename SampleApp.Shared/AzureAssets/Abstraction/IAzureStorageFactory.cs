using System.Threading.Tasks;

namespace SampleApp.Shared.AzureAssets.Abstraction
{
    public interface IAzureStorageFactory
    {
        Task SendMessage<T>(T message, string queueName);
    }
}
