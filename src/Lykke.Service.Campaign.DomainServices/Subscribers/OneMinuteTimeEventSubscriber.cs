using System;
using System.Threading.Tasks;
using Lykke.Common.Log;
using Lykke.Service.Campaign.Domain.Services;
using Lykke.Service.Scheduler.Contract.Events;

namespace Lykke.Service.Campaign.DomainServices.Subscribers
{
    public class OneMinuteTimeEventSubscriber : RabbitSubscriber<TimeEvent>
    {
        private readonly ICampaignService _campaignService;

        public OneMinuteTimeEventSubscriber(
            string connectionString,
            string exchangeName,
            ILogFactory logFactory, ICampaignService campaignService)
            : base(connectionString, exchangeName, logFactory, false)
        {
            _campaignService = campaignService ??
                               throw new ArgumentNullException(nameof(campaignService));
        }

        protected override async Task<(bool isSuccessful, string errorMessage)> ProcessMessageAsync(TimeEvent message)
        {
            return await _campaignService.ProcessOneMinuteTimeEvent(DateTime.UtcNow);
        }
    }
}
