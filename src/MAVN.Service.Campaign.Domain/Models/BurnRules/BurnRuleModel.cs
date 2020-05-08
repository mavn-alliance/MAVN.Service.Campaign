using System;
using System.Collections.Generic;
using System.Linq;
using MAVN.Numerics;
using MAVN.Service.Campaign.Domain.Enums;
using MAVN.Service.PartnerManagement.Client.Models;

namespace MAVN.Service.Campaign.Domain.Models.BurnRules
{
    /// <summary>
    /// Represents burn rule.
    /// </summary>
    public class BurnRuleModel
    {
        /// <summary>
        /// The unique identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The burn rule title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The additional information.
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
        /// The date and time of creation.
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// The unique identifier of creator.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Indicates that the burn rule deleted.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// The vertical that associated with burn rule.
        /// </summary>
        public Vertical? Vertical { get; set; }

        /// <summary>
        /// The common field to store price. It could be used for voucher price <see cref="PartnerManagement.Client.Models.Vertical.Retail"/>.
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// The collection of associated partners.
        /// </summary>
        public List<Guid> PartnerIds { get; set; }
            = new List<Guid>();

        /// <summary>
        /// The collection of contents.
        /// </summary>
        public IReadOnlyList<BurnRuleContentModel> BurnRuleContents { get; set; }

        /// <summary>
        /// Indicates the order of the burn rule.
        /// </summary>
        public int Order { get; set; }

        public BurnRuleContentModel GetContent(RuleContentType contentType, Localization language)
        {
            if (BurnRuleContents == null)
                return null;

            var content = BurnRuleContents
                .FirstOrDefault(o => o.RuleContentType == contentType && o.Localization == language);

            if (content != null)
                return content;

            return BurnRuleContents
                .FirstOrDefault(o => o.RuleContentType == contentType && o.Localization == Localization.En);
        }
    }
}
