using System.Collections.Generic;

namespace Lykke.Service.Campaign.Domain.Models
{
    public class RewardRatioAttributeModel
    {
        public IReadOnlyList<RatioAttributeModel> Ratios { get; set; }
    }
}
