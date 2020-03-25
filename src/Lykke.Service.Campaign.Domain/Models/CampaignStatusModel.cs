using System;
using Lykke.Service.Campaign.Domain.Enums;

namespace Lykke.Service.Campaign.Domain.Models
{
    public class CampaignStatusModel
    {
        public Guid CampaignId { get; set; }

        public CampaignStatus CampaignStatus { get; set; }
    }
}
