using Lykke.SettingsReader.Attributes;

namespace MAVN.Service.Campaign.Settings
{
    public class RabbitMqSettings
    {
        [AmqpCheck]
        public string PublishRabbitMqConnectionString { get; set; }

        [AmqpCheck]
        public string SubscribeRabbitMqConnectionString { get; set; }
    }
}
