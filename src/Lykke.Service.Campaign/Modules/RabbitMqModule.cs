using Autofac;
using JetBrains.Annotations;
using Lykke.Common;
using Lykke.RabbitMqBroker.Publisher;
using Lykke.Service.Campaign.Contract.Events;
using Lykke.Service.Campaign.DomainServices.Subscribers;
using Lykke.Service.Campaign.Settings;
using Lykke.SettingsReader;

namespace Lykke.Service.Campaign.Modules
{
    [UsedImplicitly]
    public class RabbitMqModule : Module
    {
        private readonly RabbitMqSettings _settings;

        private const string CampaignChangeEventExchangeName = "lykke.campaign.campaignchange";
        private const string SpendRuleChangeEventExchangeName = "lykke.campaign.spendrulechange";
        private const string OneMinuteTimeEventExchangeName = "lykke.scheduler.oneminutesevent";
        private const string BonusTypeDetectedEventExchangeName = "lykke.bonus.types";

        public RabbitMqModule(IReloadingManager<AppSettings> appSettings)
        {
            _settings = appSettings.CurrentValue.CampaignService.RabbitMq;
        }

        protected override void Load(ContainerBuilder builder)
        {
            //Publishers
            builder.RegisterJsonRabbitPublisher<CampaignChangeEvent>(
                _settings.PublishRabbitMqConnectionString,
                CampaignChangeEventExchangeName);

            builder.RegisterJsonRabbitPublisher<SpendRuleChangedEvent>(
                _settings.PublishRabbitMqConnectionString,
                SpendRuleChangeEventExchangeName);

            //Subscribers
            builder.RegisterType<OneMinuteTimeEventSubscriber>()
                .As<IStartStop>()
                .SingleInstance()
                .WithParameter("connectionString", _settings.SubscribeRabbitMqConnectionString)
                .WithParameter("exchangeName", OneMinuteTimeEventExchangeName);

            builder.RegisterType<BonusTypeDetectedEventSubscriber>()
                .As<IStartStop>()
                .SingleInstance()
                .WithParameter("connectionString", _settings.SubscribeRabbitMqConnectionString)
                .WithParameter("exchangeName", BonusTypeDetectedEventExchangeName);
        }
    }
}
