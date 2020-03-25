using Lykke.Service.Campaign.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Lykke.Service.Campaign.MsSqlRepositories.Entities
{
    [Table("condition_attribute")]
    public class ConditionAttributeEntity : BaseEntity
    {
        [Column("condition_id")]
        public Guid ConditionId { get; set; }

        [Column("type")]
        public ConditionAttributeType Type { get; set; }

        [Column("json_value")]
        public string JsonValue { get; set; }

        [NotMapped]
        public object Value
        {
            get => JsonConvert.DeserializeObject(JsonValue);
            set => JsonConvert.SerializeObject(value);
        }

        public ConditionEntity Condition { get; set; }
    }
}
