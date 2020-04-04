using JetBrains.Annotations;
using MAVN.Service.Campaign.Client.Models.Enums;
using MAVN.Service.Campaign.Client.Models.Files.Requests;

namespace MAVN.Service.Campaign.Client.Models.BurnRuleContent
{
    /// <summary>
    ///  Represents Create burn Rule Content request model
    /// </summary>
    [PublicAPI]
    public class BurnRuleContentCreateRequest
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
