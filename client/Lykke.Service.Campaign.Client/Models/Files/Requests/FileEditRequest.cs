using System;

namespace Lykke.Service.Campaign.Client.Models.Files.Requests
{
    /// <summary>
    /// Represents file edit request model
    /// </summary>
    public class FileEditRequest
    {
        /// <summary>
        /// Represents the identifier of the file
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Represents the identifier of file's rule content (earn or spend) 
        /// </summary>
        public Guid RuleContentId { get; set; }

        /// <summary>
        /// File's name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// File's type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// File's content
        /// </summary>
        public byte[] Content { get; set; }
    }
}
