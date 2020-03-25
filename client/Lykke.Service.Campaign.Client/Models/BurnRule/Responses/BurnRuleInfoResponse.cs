using System;
using Falcon.Numerics;
using JetBrains.Annotations;
using Lykke.Service.PartnerManagement.Client.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Lykke.Service.Campaign.Client.Models.BurnRule.Responses
{
    /// <summary>
    /// burn rule information response 
    /// </summary>
    [PublicAPI]
    public class BurnRuleInfoResponse
    {
        /// <summary>
        /// Represents Rule title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Represents burn Rule's identifier
        /// </summary>
        public Guid Id { get; set; }

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
        /// Vertical to which Burn Rule belongs
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public Vertical? Vertical { get; set; }

        /// <summary>
        /// The common field to store price. It could be used for voucher price <see cref="PartnerManagement.Client.Models.Vertical.Retail"/>.
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// Represents burn rule's creation date
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Indicates the order of the burn rule.
        /// </summary>
        public int Order { get; set; }
    }
}
