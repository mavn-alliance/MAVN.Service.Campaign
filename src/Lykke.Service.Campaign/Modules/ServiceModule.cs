using Autofac;
using JetBrains.Annotations;
using Lykke.Sdk;
using Lykke.Service.Campaign.Domain.Services;
using Lykke.Service.Campaign.DomainServices.Services;
using Lykke.Service.Campaign.Managers;
using Lykke.Service.Campaign.Settings;
using Lykke.SettingsReader;

namespace Lykke.Service.Campaign.Modules
{
    [UsedImplicitly]
    public class ServiceModule : Module
    {
        private readonly IReloadingManager<AppSettings> _appSettings;

        public ServiceModule(IReloadingManager<AppSettings> appSettings)
        {
            _appSettings = appSettings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CampaignService>()
                .As<ICampaignService>()
                .SingleInstance();
            builder.RegisterType<CampaignValidationService>()
                .As<ICampaignValidationService>();
            builder.RegisterType<ConditionValidationService>()
                .As<IConditionValidationService>()
                .SingleInstance();
            builder.RegisterType<ConditionService>()
                .As<IConditionService>()
                .SingleInstance();
            builder.RegisterType<BonusTypeService>()
                .As<IBonusTypeService>()
                .SingleInstance();
            builder.RegisterType<BonusTypeValidationService>()
                .As<IBonusTypeValidationService>()
                .SingleInstance();
            builder.RegisterType<BurnRuleService>()
                .As<IBurnRuleService>()
                .WithParameter("assetName", _appSettings.CurrentValue.CampaignService.BaseCurrencyCode)
                .SingleInstance();
            builder.RegisterType<FileService>()
                .As<IFileService>()
                .SingleInstance();
            builder.RegisterType<RuleContentValidationService>()
                .As<IRuleContentValidationService>();

            builder.RegisterType<StartupManager>()
                .As<IStartupManager>()
                .SingleInstance();

            builder.RegisterType<ShutdownManager>()
                .As<IShutdownManager>()
                .AutoActivate()
                .SingleInstance();

            builder.RegisterModule(new MsSqlRepositories.AutofacModule(_appSettings.CurrentValue.CampaignService.Db.MsSqlConnectionString));

            builder.RegisterModule(new AzureRepositories.AutofacModule(_appSettings.Nested(s => s.CampaignService.Db.RulesImageConnString)));
        }
    }
}
