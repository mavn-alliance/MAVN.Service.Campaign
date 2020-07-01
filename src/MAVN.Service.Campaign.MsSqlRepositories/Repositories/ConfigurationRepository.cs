using System;
using System.Threading.Tasks;
using MAVN.Persistence.PostgreSQL.Legacy;
using MAVN.Service.Campaign.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MAVN.Service.Campaign.MsSqlRepositories.Repositories
{
    public class ConfigurationRepository : IConfigurationRepository
    {
        private readonly PostgreSQLContextFactory<CampaignContext> _msSqlContextFactory;

        public ConfigurationRepository(PostgreSQLContextFactory<CampaignContext> msSqlContextFactory)
        {
            _msSqlContextFactory = msSqlContextFactory ??
                throw new ArgumentNullException(nameof(msSqlContextFactory));
        }

        public async Task<DateTime?> Get()
        {
            using (var context = _msSqlContextFactory.CreateDataContext())
            {
                var date = await context.Configuration.FirstOrDefaultAsync();

                return date?.LastProcessedDate;
            }
        }

        public async Task Set(DateTime lastProcessedDate)
        {
            using (var context = _msSqlContextFactory.CreateDataContext())
            {
                var config =  await context.Configuration.FirstOrDefaultAsync();

                if (config == null)
                {
                    context.Configuration.Add(new Entities.Configuration() { LastProcessedDate = lastProcessedDate });
                }
                else
                {
                    config.LastProcessedDate = lastProcessedDate;
                    context.Configuration.Update(config);
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
