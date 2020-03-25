using JetBrains.Annotations;
using Lykke.Service.Campaign.Client.Models.Enums;
using Lykke.Service.Campaign.Client.Models.Files.Requests;

namespace Lykke.Service.Campaign.Client.Models.EarnRuleContent
{
    /// <summary>
    ///  Represents Create Earn Rule Content request model
    /// </summary>
    [PublicAPI]
    public class EarnRuleContentCreateRequest
    {
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
    }
}
