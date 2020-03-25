using System;
using JetBrains.Annotations;
using Lykke.Service.Campaign.Client.Models.Enums;
using Lykke.Service.Campaign.Client.Models.Files.Responses;

namespace Lykke.Service.Campaign.Client.Models.BurnRuleContent
{
    /// <summary>
    ///  Represents Base burn Rule Content model
    /// </summary>
    [PublicAPI]
    public class BurnRuleContentResponse
    {
        /// <summary>
        /// Represents content's identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Represents the type of the content
        /// </summary>
        public RuleContentType RuleContentType { get; set; }

        /// <summary>
        /// Represents content's language 
        /// </summary>
        public Localization Localization { get; set; }

        /// <summary>
        /// Represents content's value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Represents content's image
        /// </summary>
        public FileResponse Image { get; set; }
    }
}
