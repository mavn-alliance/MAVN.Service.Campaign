using Lykke.Service.Campaign.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lykke.Service.Campaign.MsSqlRepositories.Entities
{
    [Table("earn_rule_contents")]
    public class EarnRuleContentEntity : BaseEntity
    {
        [Column("campaign_id")]
        public Guid CampaignId { get; set; }

        [Column("content_type")]
        public RuleContentType RuleContentType { get; set; }

        [Column("localization")]
        public Localization Localization { get; set; }

        [Column("value")]
        public string Value { get; set; }

        // TODO: Rename to EarnRule
        public CampaignEntity Campaign { get; set; }
    }
}
