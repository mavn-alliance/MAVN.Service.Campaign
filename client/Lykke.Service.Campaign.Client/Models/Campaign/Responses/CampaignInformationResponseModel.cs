using System;
using JetBrains.Annotations;
using Lykke.Service.Campaign.Client.Models.Enums;

namespace Lykke.Service.Campaign.Client.Models.Campaign.Responses
{
    /// <summary>
    /// Model containing campaign's information
    /// </summary>
    [PublicAPI]
    public class CampaignInformationResponseModel
    {
        /// <summary>
        ///  Represents Identifier of the Campaign
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Represents Campaign name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Represents status of the Campaign
        /// </summary>
        public CampaignStatus CampaignStatus { get; set; }

        /// <summary>
        /// Represents boolean value that determines if a campaign is deleted
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Indicates the order of the burn rule.
        /// </summary>
        public int Order { get; set; }
    }
}
