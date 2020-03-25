using System;
using System.Threading.Tasks;
using Lykke.Common.Log;
using Lykke.Service.BonusTriggerAgent.Contract.Events;
using Lykke.Service.Campaign.Domain.Models;
using Lykke.Service.Campaign.Domain.Services;

namespace Lykke.Service.Campaign.DomainServices.Subscribers
{
    public class BonusTypeDetectedEventSubscriber : RabbitSubscriber<BonusTypeDetectedEvent>
    {
        private readonly IBonusTypeService _bonusTypeService;

        public BonusTypeDetectedEventSubscriber(
            string connectionString,
            string exchangeName,
            ILogFactory logFactory,
            IBonusTypeService bonusTypeService)
            : base(connectionString, exchangeName, logFactory)
        {
            _bonusTypeService = bonusTypeService;
        }

        protected override async Task<(bool isSuccessful, string errorMessage)> ProcessMessageAsync(
            BonusTypeDetectedEvent message)
        {
            await _bonusTypeService.InsertOrUpdateAsync(new BonusType
            {
                Type = message.EventName,
                DisplayName = message.DisplayName,
                AllowInfinite = message.AllowInfinite,
                AllowPercentage = message.AllowPercentage,
                AllowConversionRate = message.AllowConversionRate, 
                CreationDate = DateTime.UtcNow,
                IsAvailable = message.IsEnabled,
                Vertical = message.Vertical,
                IsStakeable = message.IsStakeable,
                IsHidden = message.IsHidden,
                Order = message.Order,
                RewardHasRatio = message.RewardHasRatio
            });

            return (true, null);
        }
    }
}
