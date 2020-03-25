using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lykke.Common.MsSql;
using Lykke.Service.Campaign.Domain.Repositories;
using Lykke.Service.Campaign.MsSqlRepositories.Entities;

namespace Lykke.Service.Campaign.MsSqlRepositories.Repositories
{
    public class BurnRulePartnerRepository : IBurnRulePartnerRepository
    {
        private readonly MsSqlContextFactory<CampaignContext> _msSqlContextFactory;

        public BurnRulePartnerRepository(MsSqlContextFactory<CampaignContext> msSqlContextFactory)
        {
            _msSqlContextFactory = msSqlContextFactory ??
                                   throw new ArgumentNullException(nameof(msSqlContextFactory));
        }

        public async Task DeleteAsync(IEnumerable<Guid> partners, Guid burnRuleId)
        {
            using (var context = _msSqlContextFactory.CreateDataContext())
            {
                var entities = context.BurnRulePartners
                    .Where(p => partners.Any(g => g == p.PartnerId) && p.BurnRuleEntityId == burnRuleId);

                context.BurnRulePartners.RemoveRange(entities);

                await context.SaveChangesAsync();
            }
        }

        public async Task InsertAsync(IEnumerable<Guid> partners, Guid burnRuleId)
        {
            using (var context = _msSqlContextFactory.CreateDataContext())
            {
                var entities = partners.Select(p => new BurnRulePartnerEntity
                {
                    BurnRuleEntityId = burnRuleId,
                    PartnerId = p
                });

                context.AddRange(entities);

                await context.SaveChangesAsync();
            }
        }
    }
}
