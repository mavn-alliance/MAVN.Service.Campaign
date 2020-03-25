using System.ComponentModel;
using JetBrains.Annotations;
using Lykke.Service.Campaign.Client.Models.Enums;

namespace Lykke.Service.Campaign.Client.Models.Campaign.Requests
{
    /// <inheritdoc />
    [PublicAPI]
    public class CampaignsPaginationRequestModel : BasePaginationRequestModel
    {
        /// <summary>
        /// Represents search field by campaign's name
        /// </summary>
        public string CampaignName { get; set; }

        /// <summary>
        ///  Represents search field by campaign's condition type
        /// </summary>
        public string  ConditionType { get; set; }

        /// <summary>
        /// Filter by campaign's status
        /// </summary>
        public CampaignStatus? CampaignStatus{ get; set; }
    }
}
