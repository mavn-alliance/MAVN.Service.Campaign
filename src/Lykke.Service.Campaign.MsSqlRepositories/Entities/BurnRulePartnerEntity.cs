using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lykke.Service.Campaign.MsSqlRepositories.Entities
{
    [Table("burn_rule_partners")]
    public class BurnRulePartnerEntity : BaseEntity
    {
        [Column("burn_rule_id")]
        public Guid BurnRuleEntityId { get; set; }

        [Column("partner_id")]
        public Guid PartnerId { get; set; }

        public BurnRuleEntity BurnRuleEntity { get; set; }
    }
}
