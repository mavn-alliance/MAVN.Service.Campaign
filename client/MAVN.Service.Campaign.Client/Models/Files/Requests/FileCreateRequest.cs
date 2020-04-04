using System;

namespace MAVN.Service.Campaign.Client.Models.Files.Requests
{
    /// <summary>
    /// Represents file's create request model
    /// </summary>
    public class FileCreateRequest
    {
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
