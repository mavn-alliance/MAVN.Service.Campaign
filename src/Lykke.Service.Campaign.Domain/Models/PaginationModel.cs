namespace Lykke.Service.Campaign.Domain.Models
{
    public class PaginationModel
    {
        public int CurrentPage { get; set; }
            = 1;

        public int PageSize { get; set; }
            = 500;

        public int TotalCount { get; set; }
    }
}
