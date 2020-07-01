using System.Linq;
using System.Threading.Tasks;
using MAVN.Persistence.PostgreSQL.Legacy;
using MAVN.Service.Campaign.Domain.Models;
using MAVN.Service.Campaign.MsSqlRepositories;
using MAVN.Service.Campaign.MsSqlRepositories.Repositories;
using MAVN.Service.Campaign.Tests.MsSqlRepositories.Fixtures;
using Xunit;

namespace MAVN.Service.Campaign.Tests.MsSqlRepositories
{
    public class CampaignRepositoryTest : IClassFixture<CampaignContextFixture>
    {
        private readonly CampaignRepository _campaignRepository;

        public CampaignRepositoryTest()
        {
            var mapper = MapperHelper.CreateAutoMapper();

            var contextFixture = new CampaignContextFixture();

            var bonusEngineContext = contextFixture.BonusEngineContext;

            var postgresContextFactory = new PostgreSQLContextFactory<CampaignContext>(
                dbCtx => bonusEngineContext, contextFixture.DbContextOptions);

            _campaignRepository = new CampaignRepository(postgresContextFactory, mapper);
        }

        #region GetPagedCampaignsAsync

        [Fact]
        public async Task ShouldReturnNotDeletedCampaignsWithDefaultPagerValues_WhenNoParametersPassed()
        {
            //Act
            var result = await _campaignRepository.GetPagedCampaignsAsync(new CampaignListRequestModel());

            //Assert
            Assert.IsType<PaginatedCampaignListModel>(result);
            Assert.Equal(1, result.CurrentPage);
            Assert.Equal(500, result.PageSize);
            Assert.Equal(5, result.TotalCount);
            Assert.NotEmpty(result.Campaigns);
        }

        [Fact]
        public async Task ShouldReturnNotDeletedCampaignsWithPager_WhenPagerParametersPassed()
        {
            //Act
            var result = await _campaignRepository.GetPagedCampaignsAsync(new CampaignListRequestModel()
            {
                CurrentPage = 2,
                PageSize = 2
            });

            //Assert
            Assert.IsType<PaginatedCampaignListModel>(result);
            Assert.Equal(2, result.CurrentPage);
            Assert.Equal(2, result.PageSize);
            Assert.Equal(5, result.TotalCount);
            Assert.NotEmpty(result.Campaigns);
            Assert.Equal(2, result.Campaigns.Count());
            Assert.Empty(result.Campaigns.Where(c => c.IsDeleted));
        }

        [Fact]
        public async Task ShouldReturnNotDeletedActiveCampaignsOrderedDescByVerticalOrderAndCreateDate_WhenFilteredByActiveStatusAndSortByName()
        {
            //Act
            var result = await _campaignRepository.GetPagedCampaignsAsync(new CampaignListRequestModel()
            {
                CampaignStatus = Domain.Enums.CampaignStatus.Active
            });

            //Assert
            Assert.IsType<PaginatedCampaignListModel>(result);
            Assert.Equal(1, result.CurrentPage);
            Assert.Equal(500, result.PageSize);
            Assert.Equal(2, result.TotalCount);
            Assert.NotEmpty(result.Campaigns);
            Assert.Equal(2, result.Campaigns.Count());

            //Assert sorting and filtering
            Assert.Equal("ActiveCampaign", result.Campaigns.First().Name);
            Assert.Equal("ActiveWithoutEndDate", result.Campaigns.Last().Name);
        }

        [Fact]
        public async Task ShouldReturnNotDeletedPendingCampaignsOrderedDescByName_WhenFilteredByPendingStatus()
        {
            //Act
            var result = await _campaignRepository.GetPagedCampaignsAsync(new CampaignListRequestModel()
            {
                CampaignStatus = Domain.Enums.CampaignStatus.Pending
            });

            //Assert
            Assert.IsType<PaginatedCampaignListModel>(result);
            Assert.Equal(1, result.CurrentPage);
            Assert.Equal(500, result.PageSize);
            Assert.Equal(1, result.TotalCount);
            Assert.NotEmpty(result.Campaigns);
            Assert.Single(result.Campaigns);

            //Assert sorting and filtering
            Assert.Equal("PendingCampaign", result.Campaigns.First().Name);
        }

        [Fact]
        public async Task ShouldReturnNotDeletedCompletedCampaigns_WhenFilteredByCompletedStatus()
        {
            //Act
            var result = await _campaignRepository.GetPagedCampaignsAsync(new CampaignListRequestModel()
            {
                CampaignStatus = Domain.Enums.CampaignStatus.Completed
            });

            //Assert
            Assert.IsType<PaginatedCampaignListModel>(result);
            Assert.Equal(1, result.CurrentPage);
            Assert.Equal(500, result.PageSize);
            Assert.Equal(1, result.TotalCount);
            Assert.NotEmpty(result.Campaigns);
            Assert.Single(result.Campaigns);

            //Assert sorting and filtering
            Assert.Equal("CompletedCampaign", result.Campaigns.First().Name);
        }

        [Fact]
        public async Task ShouldReturnNotDeletedCampaignsWithGivenConditionType_WhenFilteredByBonusType()
        {
            //Act
            var result = await _campaignRepository.GetPagedCampaignsAsync(new CampaignListRequestModel()
            {
                ConditionType = "signup"
            });

            //Assert
            Assert.IsType<PaginatedCampaignListModel>(result);
            Assert.Equal(1, result.CurrentPage);
            Assert.Equal(500, result.PageSize);
            Assert.Equal(1, result.TotalCount);
            Assert.NotEmpty(result.Campaigns);
            Assert.Single(result.Campaigns);

            //Assert sorting and filtering
            Assert.Equal("ActiveCampaign", result.Campaigns.First().Name);
        }
        #endregion

        #region IsDeleted Tests

        [Fact]
        public async Task ShouldNotGetCampaignById_WhenCampaignIsMarkedAsIsDeleted()
        {
            //Act
            var result = await _campaignRepository.GetCampaignAsync(CampaignDbContextSeed.CampaignEntities[0].Id);

            //Assert
            Assert.Null(result);
        }


        [Fact]
        public async Task ShouldNotGetActiveCampaign_WhenCampaignIsMarkedAsIsDeleted()
        {
            //Act
            var result = await _campaignRepository.GetActiveCampaignsAsync();

            //Assert
            Assert.DoesNotContain(result, r => r.Id == CampaignDbContextSeed.CampaignEntities[0].Id.ToString("D"));
        }

        [Fact]
        public async Task ShouldNotAllWithConditionType_WhenCampaignIsMarkedAsIsDeleted()
        {
            //Act
            var result = await _campaignRepository.GetAllWithConditionTypeAsync(CampaignDbContextSeed.BonusTypeSignUp, true);

            //Assert
            Assert.DoesNotContain(result, r => r.Id == CampaignDbContextSeed.CampaignEntities[0].Id.ToString("D"));
        }

        [Fact]
        public async Task ShouldNotAllCampaigns_WhenCampaignIsMarkedAsIsDeleted()
        {
            //Act
            var result = await _campaignRepository.GetCampaignsAsync();

            //Assert
            Assert.DoesNotContain(result, r => r.Id == CampaignDbContextSeed.CampaignEntities[0].Id.ToString("D"));
        }

        [Fact]
        public async Task ShouldGetCampaignsByIds_WhenCampaignIsMarkedAsIsDeleted()
        {
            //Act
            var result = await _campaignRepository.GetCampaignsByIdsAsync(new [] { CampaignDbContextSeed.CampaignEntities[0].Id });

            //Assert
            Assert.Contains(result, r => r.Id == CampaignDbContextSeed.CampaignEntities[0].Id.ToString("D"));
        }

        [Fact]
        public async Task ShouldNotGetPagedCampaigns_WhenCampaignIsMarkedAsIsDeleted()
        {
            //Act
            var result = await _campaignRepository.GetPagedCampaignsAsync(new CampaignListRequestModel
            {
                CampaignName = "DeletedCampaign"
            });

            //Assert
            Assert.DoesNotContain(result.Campaigns, c => c.Id == CampaignDbContextSeed.CampaignEntities[0].Id.ToString("D"));
        }

        #endregion
    }
}
