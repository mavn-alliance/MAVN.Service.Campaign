using System;
using System.Collections.Generic;
using Falcon.Numerics;
using MAVN.Service.Campaign.Domain.Enums;
using Lykke.Service.PartnerManagement.Client.Models;

namespace MAVN.Service.Campaign.Domain.Models
{
    public class ConditionLocalizedModel
    {
        public string Id { get; set; }

        public string Type { get; set; }

        public Vertical? Vertical { get; set; }

        public bool IsHidden { get; set; }

        public string DisplayName { get; set; }

        public Money18 ImmediateReward { get; set; }

        public int CompletionCount { get; set; }

        public List<Guid> PartnerIds { get; set; }

        public bool HasStaking { get; set; }

        public Money18? StakeAmount { get; set; }

        public int? StakingPeriod { get; set; }

        public int? StakeWarningPeriod { get; set; }

        public decimal? StakingRule { get; set; }

        public decimal? BurningRule { get; set; }

        public RewardType RewardType { get; set; }

        public bool IsApproximate { get; set; }

        public Money18? ApproximateAward { get; set; }

        public Money18? AmountInTokens { get; set; }

        public decimal? AmountInCurrency { get; set; }

        public bool UsePartnerCurrencyRate { get; set; }

        public RewardRatioAttributeModel RewardRatio { get; set; }
    }
}
