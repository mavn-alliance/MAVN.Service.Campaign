namespace Lykke.Service.Campaign.Domain.Models
{
    public class RatioAttributeModel
    {
        public int Order { get; set; }

        public decimal RewardRatio { get; set; }

        public decimal PaymentRatio { get; set; }

        public decimal Threshold { get; set; }

        public bool AreEqual(RatioAttributeModel ratio)
            => Order == ratio.Order &&
               PaymentRatio == ratio.PaymentRatio &&
               RewardRatio == ratio.RewardRatio;
    }
}
