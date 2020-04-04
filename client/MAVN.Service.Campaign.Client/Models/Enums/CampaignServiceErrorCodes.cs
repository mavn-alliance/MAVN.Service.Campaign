namespace MAVN.Service.Campaign.Client.Models.Enums
{
    /// <summary>
    /// Represents BonusEngine's error codes
    /// </summary>
    public enum CampaignServiceErrorCodes
    {
        /// <summary>
        /// Empty code
        /// </summary>
        None,

        /// <summary>
        /// Entity with the provided id does not exists
        /// </summary>
        EntityNotFound,

        /// <summary>
        /// Passed values can not be parsed to guid
        /// </summary>
        GuidCanNotBeParsed,

        /// <summary>
        /// Entity not valid error code
        /// </summary>
        EntityNotValid,

        /// <summary>
        /// Entity already exists error code
        /// </summary>
        EntityAlreadyExists,

        /// <summary>
        /// File's format not valid
        /// </summary>
        NotValidFileFormat,

        /// <summary>
        /// Passed rule content type is not valid
        /// </summary>
        NotValidRuleContentType
    }
}
