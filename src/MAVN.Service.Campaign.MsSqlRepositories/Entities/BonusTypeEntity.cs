using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Lykke.Service.PartnerManagement.Client.Models;

namespace MAVN.Service.Campaign.MsSqlRepositories.Entities
{
    [Table("bonus_type")]
    public class BonusTypeEntity
    {
        [Key]
        [Column("type", TypeName = "varchar(64)")]
        public string Type { get; set; }

        [Column("display_name")]
        public string DisplayName { get; set; }

        [Column("is_available")]
        public bool IsAvailable { get; set; }
        
        [Column("vertical")]
        public Vertical? Vertical { get; set; }
        
        [Column("allow_infinite")]
        public bool AllowInfinite { get; set; }
        
        [Column("allow_percentage")]
        public bool AllowPercentage { get; set; }

        [Column("is_stakeable")]
        public bool IsStakeable { get; set; }

        [Column("allow_conversion_rate")]
        public bool AllowConversionRate { get; set; }

        [Column("creation_date")]
        public DateTime CreationDate { get; set; }

        [Column("is_hidden")]
        public bool IsHidden { get; set; }

        [Column("order")]
        public int Order { get; set; }

        [Column("reward_has_ration")]
        public bool RewardHasRatio { get; set; }

        public ICollection<ConditionEntity> ConditionEntities { get; set; }
    }
}
