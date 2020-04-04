using System;
using Lykke.Service.PartnerManagement.Client.Models;

namespace MAVN.Service.Campaign.Domain.Models
{
    public class BonusType
    {
        public string Type { get; set; }

        public string DisplayName { get; set; }

        public Vertical? Vertical { get; set; }

        public bool AllowInfinite { get; set; }

        public bool AllowPercentage { get; set; }

        public bool AllowConversionRate { get; set; }

        public bool IsAvailable { get; set; }

        public DateTime CreationDate { get; set; }

        public bool IsStakeable { get; set; }

        public bool IsHidden { get; set; }

        public int Order { get; set; }

        public bool RewardHasRatio { get; set; }

        public bool AreEqual(BonusType bonusType)
            => Type == bonusType.Type &&
               DisplayName == bonusType.DisplayName &&
               Vertical == bonusType.Vertical &&
               AllowInfinite == bonusType.AllowInfinite &&
               AllowPercentage == bonusType.AllowPercentage &&
               AllowConversionRate == bonusType.AllowConversionRate &&
               IsAvailable == bonusType.IsAvailable &&
               IsStakeable == bonusType.IsStakeable &&
               IsHidden == bonusType.IsHidden &&
               Order == bonusType.Order && 
               RewardHasRatio == bonusType.RewardHasRatio;
    }
}
