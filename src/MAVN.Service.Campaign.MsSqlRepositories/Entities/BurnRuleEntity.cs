using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Falcon.Numerics;
using Lykke.Service.PartnerManagement.Client.Models;

namespace MAVN.Service.Campaign.MsSqlRepositories.Entities
{
    [Table("burn_rules")]
    public class BurnRuleEntity : BaseEntity
    {
        [Column("title")]
        public string Title { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("amount_in_tokens", TypeName = "nvarchar(64)")]
        public Money18? AmountInTokens { get; set; }

        [Column("amount_in_currency")]
        public decimal? AmountInCurrency { get; set; }

        [Column("use_partner_currency_rate")]
        public bool UsePartnerCurrencyRate { get; set; }

        [Column("vertical")]
        public Vertical? Vertical { get; set; }

        [Column("price")]
        public decimal? Price { get; set; }

        [Column("creation_date")]
        public DateTime CreationDate { get; set; }

        [Column("created_by")]
        public string CreatedBy { get; set; }

        [Column("is_deleted")]
        public bool IsDeleted { get; set; }

        [Column("order")]
        public int Order { get; set; }

        public ICollection<BurnRuleContentEntity> BurnRuleContents { get; set; }

        public ICollection<BurnRulePartnerEntity> BurnRulePartners { get; set; }
    }
}
