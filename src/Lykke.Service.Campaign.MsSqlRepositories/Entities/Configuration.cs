using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lykke.Service.Campaign.MsSqlRepositories.Entities
{
    [Table("configuration")]
    public class Configuration : BaseEntity
    {
        public DateTime LastProcessedDate { get; set; }
    }
}
