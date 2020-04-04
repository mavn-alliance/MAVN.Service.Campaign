using System;
using MAVN.Service.Campaign.Domain.Enums;

namespace MAVN.Service.Campaign.Domain.Models
{
    public class CampaignStatusModel
    {
        public Guid CampaignId { get; set; }

        public CampaignStatus CampaignStatus { get; set; }
    }
}
