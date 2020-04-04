using System;
using Falcon.Numerics;
using Lykke.Service.PartnerManagement.Client.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MAVN.Service.Campaign.Client.Models.BurnRule
{
    /// <summary>
    ///  Represents a base EarnRule model
    /// </summary>
    public class BurnRuleBase
    {
        /// <summary>
        /// Represents Rule name
        /// Required field (3-50 characters)
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Represents Description of the Rule
        /// Here more detailed information about the rule can be given
        /// This is plain text, and can include links
        /// Required field (3-200 characters)
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The amount in tokens to calculate rate.
        /// </summary>
        public Money18? AmountInTokens { get; set; }

        /// <summary>
        /// The amount in currency to calculate rate.
        /// </summary>
        public decimal? AmountInCurrency { get; set; }

        /// <summary>
        /// Indicates that the partner currency rate should be used to convert an amount.
        /// </summary>
        public bool UsePartnerCurrencyRate { get; set; }

        /// <summary>
        /// Represents a list with partners' identifiers of the burn rule.
        /// </summary>
        public Guid[] PartnerIds { get; set; }

        /// <summary>
        /// Vertical to which Burn Rule belongs
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public Vertical? Vertical { get; set; }

        /// <summary>
        /// The common field to store price. It could be used for voucher price <see cref="PartnerManagement.Client.Models.Vertical.Retail"/>.
        /// </summary>
        public decimal? Price { get; set; }
        
        /// <summary>
        /// Indicates the order of the burn rule.
        /// </summary>
        public int Order { get; set; }
    }
}
