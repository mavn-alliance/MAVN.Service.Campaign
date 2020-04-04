using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MAVN.Service.Campaign.MsSqlRepositories.Entities
{
    [Table("configuration")]
    public class Configuration : BaseEntity
    {
        public DateTime LastProcessedDate { get; set; }
    }
}
