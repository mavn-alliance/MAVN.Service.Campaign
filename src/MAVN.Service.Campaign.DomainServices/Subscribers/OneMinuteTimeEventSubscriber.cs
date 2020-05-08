using System;
using System.Threading.Tasks;
using Lykke.Common.Log;
using MAVN.Service.Campaign.Domain.Services;
using MAVN.Service.Scheduler.Contract.Events;

namespace MAVN.Service.Campaign.DomainServices.Subscribers
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
