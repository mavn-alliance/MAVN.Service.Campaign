using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.Campaign.Settings
{
    public class DbSettings
    {
        [AzureTableCheck]
        public string LogsConnString { get; set; }

        public string MsSqlConnectionString { get; set; }

        [AzureTableCheck]

        public string RulesImageConnString { get; set; }
    }
}
