using System.Collections.Generic;
using Lykke.Service.Campaign.Domain.Models;

namespace Lykke.Service.Campaign.DomainServices.Helpers
{
    public class CampaignDtoEqualComparer : IEqualityComparer<CampaignDto>
    {
        public bool Equals(CampaignDto x, CampaignDto y)
        {
            if (x == null || y == null)
                return false;

            return x.Id == y.Id;
        }

        public int GetHashCode(CampaignDto obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
