using System;
using System.ComponentModel.DataAnnotations.Schema;
using Lykke.Service.Campaign.Domain.Enums;

namespace Lykke.Service.Campaign.MsSqlRepositories.Entities
{
    [Table("burn_rule_contents")]
    public class BurnRuleContentEntity : BaseEntity
    {
        [Column("burn_rule_id")]
        public Guid BurnRuleId { get; set; }

        [Column("content_type")]
        public RuleContentType RuleContentType { get; set; }

        [Column("localization")]
        public Localization Localization { get; set; }

        [Column("value")]
        public string Value { get; set; }

        public BurnRuleEntity BurnRule { get; set; }
    }
}
