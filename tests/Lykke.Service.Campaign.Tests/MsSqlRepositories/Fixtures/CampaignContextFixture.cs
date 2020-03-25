using System;
using Lykke.Service.Campaign.MsSqlRepositories;
using Microsoft.EntityFrameworkCore;

namespace Lykke.Service.Campaign.Tests.MsSqlRepositories.Fixtures
{
    public class CampaignContextFixture : IDisposable
    {
        public CampaignContext BonusEngineContext => GetInMemoryContextWithSeededData();
        public DbContextOptions DbContextOptions { get; private set; }

        private CampaignContext GetInMemoryContextWithSeededData()
        {
            var context = CreateDataContext();
            CampaignDbContextSeed.Seed(context);
            return context;
        }

        private CampaignContext CreateDataContext()
        {
            DbContextOptions = new DbContextOptionsBuilder()
                .UseInMemoryDatabase(nameof(BonusEngineContext))
                .Options;

            var context = new CampaignContext(DbContextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            return context;
        }

        public void Dispose()
        {
            BonusEngineContext?.Dispose();
        }
    }
}
