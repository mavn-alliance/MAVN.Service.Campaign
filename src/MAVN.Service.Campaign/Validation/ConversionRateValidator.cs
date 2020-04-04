using System;
using MAVN.Service.Campaign.Client.Models.Enums;

namespace MAVN.Service.Campaign.Validation
{
    public static class ConversionRateValidator
    {
        public static bool ValidateConversionRate(RewardType rewardType, bool usePartnerCurrencyRate, Func<bool> fn)
        {
            if (rewardType != RewardType.ConversionRate)
                return true;

            return usePartnerCurrencyRate || fn.Invoke();
        }
    }
}
