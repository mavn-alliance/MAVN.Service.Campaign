using System;
using JetBrains.Annotations;
using MAVN.Service.Campaign.Client.Models.Enums;
using MAVN.Service.Campaign.Client.Models.Files.Responses;

namespace MAVN.Service.Campaign.Client.Models.EarnRuleContent
{
    /// <summary>
    ///  Represents Base Earn Rule Content model
    /// </summary>
    [PublicAPI]
    public class EarnRuleContentResponse
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
