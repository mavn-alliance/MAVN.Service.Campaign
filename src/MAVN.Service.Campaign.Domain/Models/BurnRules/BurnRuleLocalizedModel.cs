using System;
using MAVN.Numerics;
using MAVN.Service.PartnerManagement.Client.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MAVN.Service.Campaign.Domain.Models.BurnRules
{
    /// <summary>
    /// Represents a spend rule that contains content in a specific language. 
    /// </summary>
    public class BurnRuleLocalizedModel
    {
        /// <summary>
        /// The spend rule unique identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The spend rule localized title. 
        /// </summary>
        public string Title { get; set; }

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
        /// The name of the target currency.
        /// </summary>
        public string CurrencyName { get; set; }

        /// <summary>
        /// The spend rule localized imageUrl. 
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// The spend rule localized description. 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Represents a list with partners' identifiers of the burn rule.
        /// </summary>
        public Guid[] PartnerIds { get; set; }

        /// <summary>
        /// Vertical to which Burn Rule belongs
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public Vertical? Vertical { set; get; }

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
