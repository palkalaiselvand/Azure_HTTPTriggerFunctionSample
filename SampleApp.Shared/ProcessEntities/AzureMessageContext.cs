using SampleApp.Shared.Enums;

namespace SampleApp.Shared.ProcessEntities
{
    public class AzureMessageContext
    {
        public AssetsType AssetsType { get; set; }
        public QueueType QueueType { get; set; }
        public string QueueOrTopicName { get; set; }
        public string ConnectionString { get; set; }
    }
}
