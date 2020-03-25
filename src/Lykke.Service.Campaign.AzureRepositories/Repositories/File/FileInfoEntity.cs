using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.Campaign.AzureRepositories.Repositories.File
{
    public class FileInfoEntity : TableEntity
    {
        public FileInfoEntity()
        {
        }

        public FileInfoEntity(string partitionKey, string rowKey)
            : base(partitionKey, rowKey)
        {
        }

        public string RuleContentId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
