using System;

namespace MAVN.Service.Campaign.Domain.Models
{
    public class FileModel
    {
        public string Id { get; set; }

        public Guid RuleContentId { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public byte[] Content { get; set; }
    }
}
