using Autofac;
using Lykke.Common.MsSql;
using MAVN.Service.Campaign.Domain.Repositories;
using MAVN.Service.Campaign.MsSqlRepositories.Repositories;

namespace MAVN.Service.Campaign.MsSqlRepositories
{
    public class AutofacModule : Module
    {
        private readonly string _connectionString;

        public AutofacModule(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterMsSql(
                _connectionString,
                connString => new CampaignContext(connString, false),
                dbConn => new CampaignContext(dbConn));

            builder.RegisterType<ConditionRepository>()
                .As<IConditionRepository>()
                .SingleInstance();

            builder.RegisterType<CampaignRepository>()
                .As<ICampaignRepository>()
                .SingleInstance();

            builder.RegisterType<BonusTypeRepository>()
                .As<IBonusTypeRepository>()
                .SingleInstance();

            builder.RegisterType<ConfigurationRepository>()
                .As<IConfigurationRepository>()
                .SingleInstance();

            builder.RegisterType<BurnRuleRepository>()
                .As<IBurnRuleRepository>()
                .SingleInstance();

            builder.RegisterType<BurnRuleContentRepository>()
                .As<IBurnRuleContentRepository>()
                .SingleInstance();

            builder.RegisterType<BurnRulePartnerRepository>()
                .As<IBurnRulePartnerRepository>()
                .SingleInstance();

            builder.RegisterType<EarnRuleContentRepository>()
                .As<IEarnRuleContentRepository>()
                .SingleInstance();
        }
    }
}
