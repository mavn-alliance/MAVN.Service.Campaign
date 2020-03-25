using System;

namespace Lykke.Service.Campaign.Domain.Models
{
    public class FileResponseModel
    {
        public string Id { get; set; }

        public Guid RuleContentId { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string BlobUrl { get; set; }
    }
}
