using MAVN.Service.Campaign.Client.Models.Enums;

namespace MAVN.Service.Campaign.Client.Models
{
    /// <summary>
    /// Represents BonusEngineErrorResponse model 
    /// </summary>
    public class CampaignServiceErrorResponseModel
    {
        /// <summary>
        /// Represents error code from the operation.
        /// </summary>
        public CampaignServiceErrorCodes ErrorCode { get; set; }

        /// <summary>
        /// Represents error message from the operation.
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
