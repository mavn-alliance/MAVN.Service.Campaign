using System.Collections.Generic;

namespace MAVN.Service.Campaign.Domain.Models
{
    public class PaginatedCampaignListModel : PaginationModel
    {
        public IEnumerable<CampaignDetails> Campaigns { get; set; }
    }
}
