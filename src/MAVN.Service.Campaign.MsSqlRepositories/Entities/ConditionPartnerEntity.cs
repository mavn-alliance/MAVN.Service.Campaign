using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MAVN.Service.Campaign.MsSqlRepositories.Entities
{
    [Table("condition_partners")]
    public class ConditionPartnerEntity : BaseEntity
    {
        [Column("condition_id")]
        public Guid ConditionEntityId { get; set; }

        [Column("partner_id")]
        public Guid PartnerId { get; set; }

        public ConditionEntity ConditionEntity { get; set; }
    }
}
