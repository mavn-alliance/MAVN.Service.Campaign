namespace MAVN.Service.Campaign.Client.Models
{
    /// <summary>
    /// Model that contains base information for pagination
    /// </summary>
    public class BasePaginationResponseModel
    {
        /// <summary>
        /// Current page
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Page size
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Total count of records
        /// </summary>
        public int TotalCount { get; set; }
    }
}
