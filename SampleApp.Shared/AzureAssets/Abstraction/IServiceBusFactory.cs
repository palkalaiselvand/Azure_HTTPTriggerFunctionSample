using SampleApp.Shared.ProcessEntities;
using System.Threading.Tasks;

namespace SampleApp.Shared.AzureAssets.Abstraction
{
    public interface IServiceBusFactory
    {
        Task SendMessage<T>(T message, AzureMessageContext context);
    }
}
