using System.ComponentModel;
using MAVN.Service.Campaign.Domain.Enums;

namespace MAVN.Service.Campaign.Domain.Models
{
    public class CampaignListRequestModel : PaginationModel
    {
        /// <summary>
        /// Represents search field by campaign's name
        /// </summary>
        public string CampaignName { get; set; }

        /// <summary>
        ///  Represents search field by campaign's condition type
        /// </summary>
        public string ConditionType { get; set; }

        /// <summary>
        /// Filter by campaign's status
        /// </summary>
        public CampaignStatus? CampaignStatus { get; set; }
    }
}
