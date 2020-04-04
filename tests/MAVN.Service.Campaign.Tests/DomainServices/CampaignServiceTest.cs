using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Lykke.Logs;
using Lykke.RabbitMqBroker.Publisher;
using MAVN.Service.Campaign.Contract.Events;
using MAVN.Service.Campaign.Domain.Exceptions;
using MAVN.Service.Campaign.Domain.Models;
using MAVN.Service.Campaign.Domain.Repositories;
using MAVN.Service.Campaign.Domain.Services;
using MAVN.Service.Campaign.DomainServices.Services;
using Moq;
using Xunit;

namespace MAVN.Service.Campaign.Tests.DomainServices
{
    public class CampaignServiceTest
    {
        private CampaignService GetCampaignServiceInstance(
            ICampaignRepository campaignRepository,
            IConditionService conditionService,
            ICampaignValidationService campaignValidationService,
            IRabbitPublisher<CampaignChangeEvent> campaignChangeEventPublisher,
            IConfigurationRepository configurationRepository)
        {
            var fileServiceMock = new Mock<IFileService>();
            var spendRuleContentRepositoryMock = new Mock<IEarnRuleContentRepository>();
            var mapper = MapperHelper.CreateAutoMapper();

            return new CampaignService(
                campaignRepository,
                conditionService,
                campaignValidationService,
                EmptyLogFactory.Instance,
                campaignChangeEventPublisher,
                configurationRepository,
                fileServiceMock.Object,
                spendRuleContentRepositoryMock.Object,
                mapper);
        }

        // InsertAsync Tests
        [Fact]
        public async Task Should_CallCampaignRepositoryInsertAsync_When_CallingInsertAsync()
        {
            // Arrange
            var fixture = new Fixture();
            var campaign = fixture.Build<CampaignDetails>()
                .With(c => c.Id, String.Empty)
                .With(x => x.ToDate, DateTime.UtcNow.AddDays(5))
                .With(x => x.FromDate, DateTime.UtcNow.AddDays(3))
                .With(x => x.IsEnabled, true)
                .Create();

            var campaignRepositoryMock = new Mock<ICampaignRepository>();
            var conditionServiceMock = new Mock<IConditionService>();
            var campaignValidationServiceMock = new Mock<ICampaignValidationService>();
            var oneMinutePublisher = new Mock<IRabbitPublisher<CampaignChangeEvent>>();
            var configRepositoryMock = new Mock<IConfigurationRepository>();

            oneMinutePublisher.Setup(o => o.PublishAsync(It.IsAny<CampaignChangeEvent>()))
                .Returns(Task.CompletedTask);

            var service = GetCampaignServiceInstance(
                campaignRepositoryMock.Object,
                conditionServiceMock.Object,
                campaignValidationServiceMock.Object,
                oneMinutePublisher.Object,
                configRepositoryMock.Object);

            campaignRepositoryMock
                .Setup(c => c.InsertAsync(It.IsAny<CampaignDetails>()))
                .ReturnsAsync(Guid.NewGuid);

            // Act
            await service.InsertAsync(campaign);

            // Assert
            campaignRepositoryMock.Verify(x => x.InsertAsync(campaign));

            oneMinutePublisher.Verify(
                c => c.PublishAsync(
                    It.Is<CampaignChangeEvent>(e => e.Action == Contract.Enums.ActionType.Created
                                                    && e.Status == Contract.Enums.CampaignStatus.Pending)), Times.Once);
        }

        [Fact]
        public async Task Should_PublishCreatedActiveCampaignChange_When_CallingInsertAsyncWithActiveCampaign()
        {
            // Arrange
            var fixture = new Fixture();
            var campaign = fixture.Build<CampaignDetails>()
                .With(c => c.Id, Guid.NewGuid().ToString())
                .With(x => x.ToDate, DateTime.UtcNow.AddDays(5))
                .With(x => x.FromDate, DateTime.UtcNow)
                .With(x => x.IsEnabled, true)
                .Create();

            var campaignRepositoryMock = new Mock<ICampaignRepository>();
            var conditionServiceMock = new Mock<IConditionService>();
            var campaignValidationServiceMock = new Mock<ICampaignValidationService>();
            var oneMinutePublisher = new Mock<IRabbitPublisher<CampaignChangeEvent>>();
            var configRepositoryMock = new Mock<IConfigurationRepository>();

            oneMinutePublisher.Setup(o => o.PublishAsync(It.IsAny<CampaignChangeEvent>()))
                .Returns(Task.CompletedTask);

            var service = GetCampaignServiceInstance(
                campaignRepositoryMock.Object,
                conditionServiceMock.Object,
                campaignValidationServiceMock.Object,
                oneMinutePublisher.Object,
                configRepositoryMock.Object);

            // Act
            await service.InsertAsync(campaign);

            // Assert
            campaignRepositoryMock.Verify(x => x.InsertAsync(campaign));

            oneMinutePublisher.Verify(
                c => c.PublishAsync(
                    It.Is<CampaignChangeEvent>(e => e.Action == Contract.Enums.ActionType.Created
                                                    && e.Status == Contract.Enums.CampaignStatus.Active)), Times.Once);
        }

        // UpdateAsync Tests
        [Fact]
        public async Task Should_ThrowArgumentException_When_CallingUpdateAsyncWithNotGuidId()
        {
            // Arrange
            var fixture = new Fixture();
            var campaign = fixture.Create<CampaignDetails>();
            campaign.Id = fixture.Create<string>().Substring(0, 10);

            var campaignRepositoryMock = new Mock<ICampaignRepository>();
            var conditionServiceMock = new Mock<IConditionService>();
            var campaignValidationServiceMock = new Mock<ICampaignValidationService>();
            var oneMinutePublisher = new Mock<IRabbitPublisher<CampaignChangeEvent>>();
            var configRepositoryMock = new Mock<IConfigurationRepository>();

            oneMinutePublisher.Setup(o => o.PublishAsync(It.IsAny<CampaignChangeEvent>()))
                .Returns(Task.CompletedTask);

            var service = GetCampaignServiceInstance(
                campaignRepositoryMock.Object,
                conditionServiceMock.Object,
                campaignValidationServiceMock.Object,
                oneMinutePublisher.Object,
                configRepositoryMock.Object);

            // Act
            // Assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateAsync(campaign));

            oneMinutePublisher.Verify(
                c => c.PublishAsync(
                    It.Is<CampaignChangeEvent>(e => e.Action == Contract.Enums.ActionType.Edited)), Times.Never);
        }

        [Fact]
        public async Task Should_CallCampaignRepositoryGetCampaignAsync_When_CallingUpdateAsync()
        {
            // Arrange
            var campaignId = Guid.NewGuid();
            var fixture = new Fixture();

            var campaign = fixture.Create<CampaignDetails>();
            campaign.Id = campaignId.ToString("D");

            var dbCampaign = fixture.Create<CampaignDetails>();
            dbCampaign.Id = campaignId.ToString("D");

            var validationResult = new ValidationResult();

            var campaignRepositoryMock = new Mock<ICampaignRepository>();
            var campaignValidationServiceMock = new Mock<ICampaignValidationService>();
            var oneMinutePublisher = new Mock<IRabbitPublisher<CampaignChangeEvent>>();
            var configRepositoryMock = new Mock<IConfigurationRepository>();

            campaignRepositoryMock
                .Setup(x => x.GetCampaignAsync(campaignId))
                .Returns(Task.FromResult(dbCampaign));
            campaignValidationServiceMock
                .Setup(x => x.ValidateUpdate(campaign, dbCampaign))
                .Returns(validationResult);
            oneMinutePublisher.Setup(o => o.PublishAsync(It.IsAny<CampaignChangeEvent>()))
                .Returns(Task.CompletedTask);

            var conditionServiceMock = new Mock<IConditionService>();

            campaignRepositoryMock.Setup(x => x.GetCampaignAsync(campaignId)).Returns(Task.FromResult(dbCampaign));

            var service = GetCampaignServiceInstance(
                campaignRepositoryMock.Object,
                conditionServiceMock.Object,
                campaignValidationServiceMock.Object,
                oneMinutePublisher.Object,
                configRepositoryMock.Object);

            // Act
            await service.UpdateAsync(campaign);

            // Assert
            campaignRepositoryMock.Verify(x => x.GetCampaignAsync(campaignId));

            oneMinutePublisher.Verify(
                c => c.PublishAsync(
                    It.Is<CampaignChangeEvent>(e => e.Action == Contract.Enums.ActionType.Edited)), Times.Once);
        }

        [Fact]
        public async Task Should_ThrowEntityNotFoundException_When_CallingUpdateAsyncCampaignAndRepositoryReturnsNull()
        {
            // Arrange
            var campaignId = Guid.NewGuid();
            var fixture = new Fixture();

            var campaign = fixture.Create<CampaignDetails>();
            campaign.Id = campaignId.ToString("D");

            var campaignRepositoryMock = new Mock<ICampaignRepository>();
            var conditionServiceMock = new Mock<IConditionService>();
            var campaignValidationServiceMock = new Mock<ICampaignValidationService>();

            campaignRepositoryMock
                .Setup(x => x.GetCampaignAsync(campaignId))
                .Returns(Task.FromResult<CampaignDetails>(null));

            campaignRepositoryMock.Setup(x => x.GetCampaignAsync(campaignId)).Returns(Task.FromResult<CampaignDetails>(null));

            var oneMinutePublisher = new Mock<IRabbitPublisher<CampaignChangeEvent>>();
            var configRepositoryMock = new Mock<IConfigurationRepository>();

            var service = GetCampaignServiceInstance(
                campaignRepositoryMock.Object,
                conditionServiceMock.Object,
                campaignValidationServiceMock.Object,
                oneMinutePublisher.Object,
                configRepositoryMock.Object);

            // Act
            // Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(() => service.UpdateAsync(campaign));

            oneMinutePublisher.Verify(
                c => c.PublishAsync(
                    It.Is<CampaignChangeEvent>(e => e.Action == Contract.Enums.ActionType.Edited)), Times.Never);
        }

        [Fact]
        public async Task Should_ThrowEntityNotValidException_When_CallingUpdateAsyncCampaignAndValidationServiceReturnsInvalidStatusResponse()
        {
            // Arrange
            var campaignId = Guid.NewGuid();
            var fixture = new Fixture();
            fixture.Customize<CampaignDetails>(c =>
                c.With(x => x.Id, campaignId.ToString("D")));

            var dbCampaign = fixture.Create<CampaignDetails>();
            var updatingCampaign = fixture.Create<CampaignDetails>();

            var validationResult = new ValidationResult();
            validationResult.Add("Validation error");

            var campaignRepositoryMock = new Mock<ICampaignRepository>();
            var campaignValidationServiceMock = new Mock<ICampaignValidationService>();
            var conditionServiceMock = new Mock<IConditionService>();

            campaignRepositoryMock
                .Setup(x => x.GetCampaignAsync(campaignId))
                .Returns(Task.FromResult(dbCampaign));
            campaignValidationServiceMock
                .Setup(x => x.ValidateUpdate(updatingCampaign, dbCampaign))
                .Returns(validationResult);

            var oneMinutePublisher = new Mock<IRabbitPublisher<CampaignChangeEvent>>();
            var configRepositoryMock = new Mock<IConfigurationRepository>();

            var service = GetCampaignServiceInstance(
                campaignRepositoryMock.Object,
                conditionServiceMock.Object,
                campaignValidationServiceMock.Object,
                oneMinutePublisher.Object,
                configRepositoryMock.Object);

            // Act
            // Assert
            await Assert.ThrowsAsync<EntityNotValidException>(() => service.UpdateAsync(updatingCampaign));

            oneMinutePublisher.Verify(
                c => c.PublishAsync(
                    It.Is<CampaignChangeEvent>(e => e.Action == Contract.Enums.ActionType.Edited)), Times.Never);
        }

        [Fact]
        public async Task Should_CallConditionRepositoryDeleteAsync_When_CallingUpdateAsync()
        {
            // Arrange
            var campaignId = Guid.NewGuid();
            var conditionId = Guid.NewGuid();
            var fixture = new Fixture();

            fixture.Customize<CampaignDetails>(c =>
                c.With(x => x.Id, campaignId.ToString("D"))
                .With(x => x.ToDate, DateTime.UtcNow.AddDays(-3))
                .With(x => x.FromDate, DateTime.UtcNow.AddDays(-5))
                .With(x => x.IsEnabled, true));

            var dbCampaign = fixture.Create<CampaignDetails>();
            dbCampaign.Conditions = fixture.CreateMany<Condition>(1).ToList();
            dbCampaign.Conditions.First().Id = conditionId.ToString("D");

            var updatingCampaign = fixture.Create<CampaignDetails>();
            updatingCampaign.Conditions = fixture.CreateMany<Condition>(1).ToList();

            var validationResult = new ValidationResult();

            var campaignRepositoryMock = new Mock<ICampaignRepository>(MockBehavior.Strict);
            var conditionServiceMock = new Mock<IConditionService>(MockBehavior.Strict);
            var campaignValidationServiceMock = new Mock<ICampaignValidationService>(MockBehavior.Strict);

            campaignRepositoryMock
                .Setup(x => x.GetCampaignAsync(campaignId))
                .Returns(Task.FromResult(dbCampaign));

            campaignValidationServiceMock
                .Setup(x => x.ValidateUpdate(updatingCampaign, dbCampaign))
                .Returns(validationResult);

            conditionServiceMock.Setup(x => x.DeleteAsync(It.IsAny<IEnumerable<Condition>>()))
                .Returns(Task.CompletedTask);
            conditionServiceMock.Setup(x => x.DeleteConditionPartnersAsync(It.IsAny<IEnumerable<Guid>>()))
                .Returns(Task.CompletedTask);
            conditionServiceMock.Setup(x => x.DeleteConditionAttributesAsync(It.IsAny<IEnumerable<Guid>>()))
                .Returns(Task.CompletedTask);
            campaignRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<CampaignDetails>()))
                .Returns(Task.CompletedTask);

            var oneMinutePublisher = new Mock<IRabbitPublisher<CampaignChangeEvent>>();
            var configRepositoryMock = new Mock<IConfigurationRepository>();

            var service = GetCampaignServiceInstance(
                campaignRepositoryMock.Object,
                conditionServiceMock.Object,
                campaignValidationServiceMock.Object,
                oneMinutePublisher.Object,
                configRepositoryMock.Object);

            // Act
            await service.UpdateAsync(updatingCampaign);

            // Assert
            conditionServiceMock.Verify(x => x.DeleteAsync(dbCampaign.Conditions));

            oneMinutePublisher.Verify(
                c => c.PublishAsync(
                    It.Is<CampaignChangeEvent>(e => e.Action == Contract.Enums.ActionType.Edited)), Times.Once);
        }

        [Fact]
        public async Task Should_CallCampaignRepositoryUpdateAsync_When_CallingUpdateAsync()
        {
            // Arrange
            var campaignId = Guid.NewGuid();
            var conditionId = Guid.NewGuid();
            var fixture = new Fixture();
            fixture.Customize<CampaignDetails>(c =>
                c.With(x => x.Id, campaignId.ToString("D")));

            var dbCampaign = fixture.Create<CampaignDetails>();
            dbCampaign.Conditions = fixture.CreateMany<Condition>(1).ToList();
            dbCampaign.Conditions.First().Id = conditionId.ToString("D");

            var updatingCampaign = fixture.Create<CampaignDetails>();
            updatingCampaign.Conditions = fixture.CreateMany<Condition>(1).ToList();

            var validationResult = new ValidationResult();

            var campaignRepositoryMock = new Mock<ICampaignRepository>();
            var campaignValidationServiceMock = new Mock<ICampaignValidationService>();

            var conditionServiceMock = new Mock<IConditionService>();
            campaignRepositoryMock
                .Setup(x => x.GetCampaignAsync(campaignId))
                .Returns(Task.FromResult(dbCampaign));
            campaignValidationServiceMock
                .Setup(x => x.ValidateUpdate(updatingCampaign, dbCampaign))
                .Returns(validationResult);

            var oneMinutePublisher = new Mock<IRabbitPublisher<CampaignChangeEvent>>();
            var configRepositoryMock = new Mock<IConfigurationRepository>();

            var service = GetCampaignServiceInstance(
                campaignRepositoryMock.Object,
                conditionServiceMock.Object,
                campaignValidationServiceMock.Object,
                oneMinutePublisher.Object,
                configRepositoryMock.Object);

            // Act
            await service.UpdateAsync(updatingCampaign);

            // Assert
            campaignRepositoryMock.Verify(x => x.UpdateAsync(updatingCampaign));

            oneMinutePublisher.Verify(
                c => c.PublishAsync(
                    It.Is<CampaignChangeEvent>(e => e.Action == Contract.Enums.ActionType.Edited)), Times.Once);
        }

        // DeleteAsync Tests
        [Fact]
        public async Task Should_ThrowArgumentException_When_CallingDeleteAsyncWithNotGuidId()
        {
            // Arrange
            var fixture = new Fixture();
            var id = fixture.Create<string>().Substring(0, 10);
            var campaignRepositoryMock = new Mock<ICampaignRepository>();
            var conditionServiceMock = new Mock<IConditionService>();
            var campaignValidationServiceMock = new Mock<ICampaignValidationService>();

            var oneMinutePublisher = new Mock<IRabbitPublisher<CampaignChangeEvent>>();
            var configRepositoryMock = new Mock<IConfigurationRepository>();

            var service = GetCampaignServiceInstance(
                campaignRepositoryMock.Object,
                conditionServiceMock.Object,
                campaignValidationServiceMock.Object,
                oneMinutePublisher.Object,
                configRepositoryMock.Object);

            // Act
            // Assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.DeleteAsync(id));

            oneMinutePublisher.Verify(
                c => c.PublishAsync(
                    It.Is<CampaignChangeEvent>(e => e.Action == Contract.Enums.ActionType.Deleted)), Times.Never);
        }

        [Fact]
        public async Task Should_CallCampaignRepositoryGetCampaignAsync_When_CallingDeleteAsyncWithGuidId()
        {
            // Arrange
            var campaignId = Guid.NewGuid();
            var fixture = new Fixture();
            fixture.Customize<CampaignDetails>(c =>
                c.With(x => x.Id, campaignId.ToString("D"))
                .With(x => x.ToDate, DateTime.UtcNow.AddDays(-3))
                .With(x => x.FromDate, DateTime.UtcNow.AddDays(-5))
                .With(x => x.IsEnabled, true));

            var dbCampaign = fixture.Create<CampaignDetails>();
            var campaignRepositoryMock = new Mock<ICampaignRepository>();
            var conditionServiceMock = new Mock<IConditionService>();
            campaignRepositoryMock
                .Setup(x => x.GetCampaignAsync(campaignId))
                .Returns(Task.FromResult(dbCampaign));

            var campaignValidationServiceMock = new Mock<ICampaignValidationService>();

            var oneMinutePublisher = new Mock<IRabbitPublisher<CampaignChangeEvent>>();
            var configRepositoryMock = new Mock<IConfigurationRepository>();

            oneMinutePublisher.Setup(o => o.PublishAsync(It.IsAny<CampaignChangeEvent>()))
                .Returns(Task.CompletedTask);

            var service = GetCampaignServiceInstance(
                campaignRepositoryMock.Object,
                conditionServiceMock.Object,
                campaignValidationServiceMock.Object,
                oneMinutePublisher.Object,
                configRepositoryMock.Object);

            // Act
            await service.DeleteAsync(campaignId.ToString("D"));

            // Assert
            campaignRepositoryMock.Verify(x => x.GetCampaignAsync(campaignId));

            oneMinutePublisher.Verify(
                c => c.PublishAsync(
                    It.Is<CampaignChangeEvent>(e => e.Action == Contract.Enums.ActionType.Deleted
                                                                    && e.Status == Contract.Enums.CampaignStatus.Completed)), Times.Once);
        }

        [Fact]
        public async Task Should_NotThrowException_When_CallingDeleteAsyncCampaignAndRepositoryReturnsNull()
        {
            // Arrange
            var fixture = new Fixture();
            var campaignId = Guid.NewGuid();
            var campaign = fixture.Create<CampaignDetails>();
            campaign.Id = campaignId.ToString("D");

            var campaignRepositoryMock = new Mock<ICampaignRepository>();

            campaignRepositoryMock
                .Setup(x => x.GetCampaignAsync(campaignId))
                .Returns(Task.FromResult<CampaignDetails>(null));

            var conditionServiceMock = new Mock<IConditionService>();
            campaignRepositoryMock.Setup(x => x.GetCampaignAsync(campaignId)).Returns(Task.FromResult<CampaignDetails>(null));

            var campaignValidationServiceMock = new Mock<ICampaignValidationService>();

            var oneMinutePublisher = new Mock<IRabbitPublisher<CampaignChangeEvent>>();
            var configRepositoryMock = new Mock<IConfigurationRepository>();

            var service = GetCampaignServiceInstance(
                campaignRepositoryMock.Object,
                conditionServiceMock.Object,
                campaignValidationServiceMock.Object,
                oneMinutePublisher.Object,
                configRepositoryMock.Object);

            // Act
            await service.DeleteAsync(campaignId.ToString("D"));

            // Assert
            // Test will fail on exception
        }

        [Fact]
        public async Task Should_CallCampaignRepositoryDeleteAsync_When_CallingDeleteAsync()
        {
            // Arrange
            var campaignId = Guid.NewGuid();
            var fixture = new Fixture();
            var dbCampaign = fixture.Create<CampaignDetails>();
            dbCampaign.Id = campaignId.ToString("D");

            var campaignRepositoryMock = new Mock<ICampaignRepository>();
            var conditionServiceMock = new Mock<IConditionService>();

            campaignRepositoryMock
                .Setup(x => x.GetCampaignAsync(campaignId))
                .Returns(Task.FromResult(dbCampaign));

            var campaignValidationServiceMock = new Mock<ICampaignValidationService>();

            var oneMinutePublisher = new Mock<IRabbitPublisher<CampaignChangeEvent>>();
            var configRepositoryMock = new Mock<IConfigurationRepository>();

            var service = GetCampaignServiceInstance(
                campaignRepositoryMock.Object,
                conditionServiceMock.Object,
                campaignValidationServiceMock.Object,
                oneMinutePublisher.Object,
                configRepositoryMock.Object);

            // Act
            await service.DeleteAsync(dbCampaign.Id);

            // Assert
            campaignRepositoryMock.Verify(x => x.DeleteAsync(dbCampaign));
        }

        // GetCampaignAsync Tests
        [Fact]
        public async Task ShouldThrowArgumentException_WhenCallingGetCampaignAsyncWithNotGuidId()
        {
            // Arrange
            var fixture = new Fixture();
            var id = fixture.Create<string>().Substring(0, 10);
            var campaignRepositoryMock = new Mock<ICampaignRepository>();
            var conditionServiceMock = new Mock<IConditionService>();
            var campaignValidationServiceMock = new Mock<ICampaignValidationService>();

            var oneMinutePublisher = new Mock<IRabbitPublisher<CampaignChangeEvent>>();
            var configRepositoryMock = new Mock<IConfigurationRepository>();

            var service = GetCampaignServiceInstance(
                campaignRepositoryMock.Object,
                conditionServiceMock.Object,
                campaignValidationServiceMock.Object,
                oneMinutePublisher.Object,
                configRepositoryMock.Object);

            // Act
            // Assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.GetCampaignAsync(id));
        }

        [Fact]
        public async Task Should_CallCampaignRepositoryGetCampaignAsync_WhenCallingGetCampaignAsync()
        {
            // Arrange
            var campaignId = Guid.NewGuid();
            var fixture = new Fixture();
            var dbCampaign = fixture.Create<CampaignDetails>();
            dbCampaign.Id = campaignId.ToString("D");

            var campaignRepositoryMock = new Mock<ICampaignRepository>();
            var conditionServiceMock = new Mock<IConditionService>();
            campaignRepositoryMock
                .Setup(x => x.GetCampaignAsync(campaignId))
                .Returns(Task.FromResult(dbCampaign));

            var campaignValidationServiceMock = new Mock<ICampaignValidationService>();
            var oneMinutePublisher = new Mock<IRabbitPublisher<CampaignChangeEvent>>();
            var configRepositoryMock = new Mock<IConfigurationRepository>();

            var service = GetCampaignServiceInstance(
                campaignRepositoryMock.Object,
                conditionServiceMock.Object,
                campaignValidationServiceMock.Object,
                oneMinutePublisher.Object,
                configRepositoryMock.Object);

            // Act
            await service.GetCampaignAsync(dbCampaign.Id);

            // Assert
            campaignRepositoryMock.Verify(x => x.GetCampaignAsync(campaignId));
        }

        [Fact]
        public async Task Should_ThrowEntityNotFoundException_When_CallingGetCampaignAsyncAndCampaignRepositoryReturnsNull()
        {
            // Arrange
            var fixture = new Fixture();
            var campaignId = Guid.NewGuid();
            var campaign = fixture.Create<CampaignDetails>();
            campaign.Id = campaignId.ToString("D");

            var campaignRepositoryMock = new Mock<ICampaignRepository>();
            var conditionServiceMock = new Mock<IConditionService>();

            campaignRepositoryMock
                .Setup(x => x.GetCampaignAsync(campaignId))
                .Returns(Task.FromResult<CampaignDetails>(null));

            var campaignValidationServiceMock = new Mock<ICampaignValidationService>();
            var oneMinutePublisher = new Mock<IRabbitPublisher<CampaignChangeEvent>>();
            var configRepositoryMock = new Mock<IConfigurationRepository>();

            var service = GetCampaignServiceInstance(
                campaignRepositoryMock.Object,
                conditionServiceMock.Object,
                campaignValidationServiceMock.Object,
                oneMinutePublisher.Object,
                configRepositoryMock.Object);

            // Act
            // Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(() => service.GetCampaignAsync(campaign.Id));
        }

        // GetCampaignsAsync
        [Fact]
        public async Task Should_CallRepositoryGetCampaignsAsync_WhenCallingGetCampaignsAsync()
        {
            // Arrange
            var campaignRepositoryMock = new Mock<ICampaignRepository>();
            var conditionServiceMock = new Mock<IConditionService>();

            var campaignValidationServiceMock = new Mock<ICampaignValidationService>();
            var oneMinutePublisher = new Mock<IRabbitPublisher<CampaignChangeEvent>>();
            var configRepositoryMock = new Mock<IConfigurationRepository>();

            var service = GetCampaignServiceInstance(
                campaignRepositoryMock.Object,
                conditionServiceMock.Object,
                campaignValidationServiceMock.Object,
                oneMinutePublisher.Object,
                configRepositoryMock.Object);

            // Act
            await service.GetCampaignsAsync();

            // Assert
            campaignRepositoryMock.Verify(x => x.GetCampaignsAsync());
        }

        // GetActiveCampaignsAsync
        [Fact]
        public async Task Should_CallRepositoryGetActiveCampaignsAsync_WhenCallingGetActiveCampaignsAsync()
        {
            // Arrange
            var campaignRepositoryMock = new Mock<ICampaignRepository>();
            var conditionServiceMock = new Mock<IConditionService>();

            var campaignValidationServiceMock = new Mock<ICampaignValidationService>();

            var oneMinutePublisher = new Mock<IRabbitPublisher<CampaignChangeEvent>>();
            var configRepositoryMock = new Mock<IConfigurationRepository>();

            var service = GetCampaignServiceInstance(
                campaignRepositoryMock.Object,
                conditionServiceMock.Object,
                campaignValidationServiceMock.Object,
                oneMinutePublisher.Object,
                configRepositoryMock.Object);

            // Act
            await service.GetActiveCampaignsAsync();

            // Assert
            campaignRepositoryMock.Verify(x => x.GetActiveCampaignsAsync());
        }

        //ProcessOneMinuteTimeEvent

        /// <summary>
        /// Given that the campaigns should be processed for first time
        /// LastProcessedDate is not recorded
        /// In this case should be send event for all active campaigns
        /// that are not deleted or disabled at the current moment 
        /// </summary>
        [Fact]
        public async Task Should_PublishCampaignChangeEventForAllActiveCampaigns_WhenCallingProcessOneMinuteTimeEventAndLastProcessedIsNull()
        {
            // Arrange
            var campaignRepositoryMock = new Mock<ICampaignRepository>();
            var conditionServiceMock = new Mock<IConditionService>();
            var campaignValidationServiceMock = new Mock<ICampaignValidationService>();
            var oneMinutePublisher = new Mock<IRabbitPublisher<CampaignChangeEvent>>();
            var configRepositoryMock = new Mock<IConfigurationRepository>();

            var now = DateTime.UtcNow;
            var fixture = new Fixture();
            fixture.Customize<CampaignDto>(c =>
                c.With(cc => cc.IsDeleted, false)
                .With(cc => cc.IsEnabled, true));

            var activeCampaignsNow = fixture.CreateMany<CampaignDto>(count: 3).ToList();

            configRepositoryMock.Setup(c => c.Get())
                .ReturnsAsync(() => null);

            campaignRepositoryMock.Setup(c =>
                    c.GetAllByStatus(Domain.Enums.CampaignStatus.Active, default))
                .ReturnsAsync(new List<CampaignDto>());

            campaignRepositoryMock.Setup(c =>
                    c.GetAllByStatus(Domain.Enums.CampaignStatus.Pending, default))
                .ReturnsAsync(new List<CampaignDto>());

            campaignRepositoryMock.Setup(c =>
                    c.GetAllByStatus(Domain.Enums.CampaignStatus.Active, now))
                .ReturnsAsync(activeCampaignsNow);

            oneMinutePublisher.Setup(o => o.PublishAsync(It.IsAny<CampaignChangeEvent>()))
                .Returns(() => Task.CompletedTask);

            var service = GetCampaignServiceInstance(
                campaignRepositoryMock.Object,
                conditionServiceMock.Object,
                campaignValidationServiceMock.Object,
                oneMinutePublisher.Object,
                configRepositoryMock.Object);

            // Act
            await service.ProcessOneMinuteTimeEvent(now);

            // Assert
            campaignRepositoryMock
                .Verify(x => x.GetAllByStatus(Domain.Enums.CampaignStatus.Active, default), Times.Never);
            campaignRepositoryMock
                .Verify(x => x.GetAllByStatus(Domain.Enums.CampaignStatus.Pending, default), Times.Never);
            campaignRepositoryMock
                .Verify(x => x.GetAllByStatus(Domain.Enums.CampaignStatus.Active, now), Times.Once);
            configRepositoryMock.
                Verify(c => c.Set(It.IsAny<DateTime>()), Times.Once);
            oneMinutePublisher
                .Verify(
                    o => o.PublishAsync(It.Is<CampaignChangeEvent>(mo =>
                        mo.Status == Contract.Enums.CampaignStatus.Completed)), Times.Never);

            oneMinutePublisher
                .Verify(
                    o => o.PublishAsync(It.Is<CampaignChangeEvent>(mo =>
                        mo.Status == Contract.Enums.CampaignStatus.Active)), Times.Exactly(3));
        }


        /// <summary>
        /// Given that the campaigns should be processed and LastProcessedDate is recorded
        /// In this case should be send event for all active campaigns that are not deleted or disabled at the current moment 
        /// Also event for all completed not deleted and disabled campaigns should be published
        /// </summary>
        [Fact]
        public async Task Should_PublishCampaignChangeEventForActiveNotDeletedCampaigns_WhenCallingProcessOneMinuteTimeEvent()
        {
            // Arrange
            var campaignRepositoryMock = new Mock<ICampaignRepository>();
            var conditionServiceMock = new Mock<IConditionService>();
            var campaignValidationServiceMock = new Mock<ICampaignValidationService>();
            var oneMinutePublisher = new Mock<IRabbitPublisher<CampaignChangeEvent>>();
            var configRepositoryMock = new Mock<IConfigurationRepository>();

            var lastProcessedDate = DateTime.UtcNow.AddMinutes(-5);

            var now = DateTime.UtcNow;
            var fixture = new Fixture();

            var pendingCampaign = fixture.Build<CampaignDto>()
                .With(c => c.Id, Guid.Parse("be52c542-4574-4014-8ddc-d10f352ebe1c"))
                .With(c => c.IsDeleted, false)
                .With(c => c.IsEnabled, true)
                .Create();

            var activeNowCampaign = fixture.Build<CampaignDto>()
                .With(c => c.Id, Guid.Parse("24a31d5d-3bbc-4e33-88b7-06210ab00111"))
                .With(c => c.IsDeleted, false)
                .With(c => c.IsEnabled, true)
                .Create();

            var activeNowCampaign1 = fixture.Build<CampaignDto>()
                .With(c => c.Id, Guid.Parse("24a31d5d-3bbc-4e33-88b7-06210ab00111"))
                .With(c => c.IsDeleted, false)
                .With(c => c.IsEnabled, true)
                .Create();

            var completedOne = fixture.Build<CampaignDto>()
                .With(c => c.Id, Guid.Parse("8225f515-9d12-4be0-87e1-4b0162ebf8cf"))
                .With(c => c.IsDeleted, false)
                .With(c => c.IsEnabled, true)
                .Create();

            var completedSecond = fixture.Build<CampaignDto>()
                .With(c => c.Id, Guid.Parse("8225f515-9d12-4be0-87e1-4b0162ebf8c1"))
                .With(c => c.IsDeleted, false)
                .With(c => c.IsEnabled, true)
                .Create();

            var completedButDeleted = fixture.Build<CampaignDto>()
                .With(c => c.Id, Guid.Parse("361b845a-bbbf-4568-be4f-67de4b691531"))
                .With(c => c.IsDeleted, true)
                .With(c => c.IsEnabled, true)
                .Create();

            IReadOnlyList<CampaignDto> activeCampaignsThen = new List<CampaignDto>()
            {
                completedButDeleted,
                completedSecond,
                completedOne
            };

            IReadOnlyList<CampaignDto> pendingCampaignsThen = new List<CampaignDto>()
            {
               pendingCampaign,
               activeNowCampaign,
               fixture.Build<CampaignDto>()
                   .With(c => c.Id, Guid.Parse("be52c542-4574-4014-8ddc-d10f352ebe13"))
                   .Create()
            };

            IReadOnlyList<CampaignDto> activeCampaignsNow = new List<CampaignDto>()
            {
                fixture.Build<CampaignDto>()
                  .With(c => c.Id, Guid.Parse("24a31d5d-3bbc-4e33-88b7-06210ab001e9"))
                  .Create(),
              pendingCampaign,
              activeNowCampaign1
            };

            configRepositoryMock.Setup(c => c.Get())
                .ReturnsAsync(lastProcessedDate);

            campaignRepositoryMock.Setup(c =>
                    c.GetAllByStatus(Domain.Enums.CampaignStatus.Active, lastProcessedDate))
                .ReturnsAsync(activeCampaignsThen);

            campaignRepositoryMock.Setup(c =>
                    c.GetAllByStatus(Domain.Enums.CampaignStatus.Pending, lastProcessedDate))
                .ReturnsAsync(pendingCampaignsThen);

            campaignRepositoryMock.Setup(c =>
                    c.GetAllByStatus(Domain.Enums.CampaignStatus.Active, now))
                .ReturnsAsync(activeCampaignsNow);

            oneMinutePublisher.Setup(o => o.PublishAsync(It.IsAny<CampaignChangeEvent>()))
                .Returns(() => Task.CompletedTask);

            var service = GetCampaignServiceInstance(
                campaignRepositoryMock.Object,
                conditionServiceMock.Object,
                campaignValidationServiceMock.Object,
                oneMinutePublisher.Object,
                configRepositoryMock.Object);

            // Act
            await service.ProcessOneMinuteTimeEvent(now);

            // Assert
            campaignRepositoryMock
                .Verify(x => x.GetAllByStatus(Domain.Enums.CampaignStatus.Active, lastProcessedDate), Times.Once);
            campaignRepositoryMock
                .Verify(x => x.GetAllByStatus(Domain.Enums.CampaignStatus.Pending, lastProcessedDate), Times.Once);
            campaignRepositoryMock
                .Verify(x => x.GetAllByStatus(Domain.Enums.CampaignStatus.Active, It.IsAny<DateTime>()), Times.Exactly(2));
            configRepositoryMock.
                Verify(c => c.Set(It.IsAny<DateTime>()), Times.Once);
            oneMinutePublisher
                .Verify(
                    o => o.PublishAsync(It.Is<CampaignChangeEvent>(mo =>
                        mo.Status == Contract.Enums.CampaignStatus.Completed)), Times.Exactly(2));

            oneMinutePublisher
                .Verify(
                    o => o.PublishAsync(It.Is<CampaignChangeEvent>(mo =>
                        mo.Status == Contract.Enums.CampaignStatus.Active)), Times.Exactly(2));
        }

        [Fact]
        public async Task Should_DeleteCampaignConditionsRepositoryDeleteAsync_When_CallingUpdateAndChangingPartnersAsync()
        {
            // Arrange
            var campaignId = Guid.NewGuid();
            var conditionId = Guid.NewGuid();
            var secondConditionId = Guid.NewGuid();
            var fixture = new Fixture();
            fixture.Customize<CampaignDetails>(c =>
                c.With(x => x.Id, campaignId.ToString("D")));

            var oldPartnersIds = new List<Guid>()
            {
                Guid.Parse("9aae7587-1422-4986-a127-94332d2e5fe8"),
                Guid.Parse("2ea16a40-b990-4844-ab6d-883c5abe4cd8")
            };

            var dbCampaign = fixture.Create<CampaignDetails>();
            dbCampaign.Conditions = new List<Condition>()
            {
                fixture.Build<Condition>()
                    .With(c=>c.PartnerIds, oldPartnersIds)
                    .With(c=> c.Id, conditionId.ToString())
                    .Create(),
                fixture.Build<Condition>()
                    .With(c=>c.PartnerIds,
                        new List<Guid>()
                        {
                            Guid.Parse("9aae7587-1422-4986-a127-94332d2e5fe3")
                        })
                    .With(c=> c.Id, secondConditionId.ToString())
                    .Create()
            };

            dbCampaign.Conditions.First().Id = conditionId.ToString("D");

            var updatingCampaign = fixture.Create<CampaignDetails>();
            updatingCampaign.Conditions = new List<Condition>()
            {
                fixture.Build<Condition>()
                    .With(c=>c.PartnerIds,
                        new List<Guid>()
                        {
                            Guid.Parse("9aae7587-1422-4986-a127-94332d2e5fe5")
                        })
                    .With(c=> c.Id, conditionId.ToString())
                    .Create(),
                fixture.Build<Condition>()
                    .With(c=>c.PartnerIds,
                        new List<Guid>()
                        {
                            Guid.Parse("9aae7587-1422-4986-a127-94332d2e5fe3")
                        })
                    .With(c=> c.Id, secondConditionId.ToString())
                    .Create()
            };

            var validationResult = new ValidationResult();

            var campaignRepositoryMock = new Mock<ICampaignRepository>();
            var campaignValidationServiceMock = new Mock<ICampaignValidationService>();

            var conditionServiceMock = new Mock<IConditionService>();

            campaignRepositoryMock
                .Setup(x => x.GetCampaignAsync(campaignId))
                    .Returns(Task.FromResult(dbCampaign));

            campaignValidationServiceMock
                .Setup(x => x.ValidateUpdate(updatingCampaign, dbCampaign))
                    .Returns(validationResult);

            var oneMinutePublisher = new Mock<IRabbitPublisher<CampaignChangeEvent>>();
            var configRepositoryMock = new Mock<IConfigurationRepository>();

            var service = GetCampaignServiceInstance(
                campaignRepositoryMock.Object,
                conditionServiceMock.Object,
                campaignValidationServiceMock.Object,
                oneMinutePublisher.Object,
                configRepositoryMock.Object);

            // Act
            await service.UpdateAsync(updatingCampaign);

            // Assert
            conditionServiceMock.Verify(c => c.DeleteAsync(new List<Condition>()), times: Times.Once);
            conditionServiceMock.Verify(c => c.DeleteConditionPartnersAsync(new List<Guid>() { conditionId, secondConditionId }), times: Times.Once);

            campaignRepositoryMock.Verify(expression: x => x.UpdateAsync(updatingCampaign));

            oneMinutePublisher.Verify(
                c => c.PublishAsync(
                    It.Is<CampaignChangeEvent>(e => e.Action == Contract.Enums.ActionType.Edited)), Times.Once);
        }

        [Fact]
        public async Task Should_DeleteCampaignConditionDeleteAsync_When_CallingUpdateAndAddingNewCondition()
        {
            // Arrange
            var campaignId = Guid.NewGuid();
            var conditionId = Guid.NewGuid();
            var secondConditionId = Guid.NewGuid();
            var fixture = new Fixture();
            fixture.Customize<CampaignDetails>(c =>
                c.With(x => x.Id, campaignId.ToString("D")));

            var oldPartnersIds = new List<Guid>()
            {
                Guid.Parse("9aae7587-1422-4986-a127-94332d2e5fe8"),
                Guid.Parse("2ea16a40-b990-4844-ab6d-883c5abe4cd8")
            };

            var conditionToBeDeleted = fixture.Build<Condition>()
                .With(c => c.PartnerIds,
                    new List<Guid>() {Guid.Parse("9aae7587-1422-4986-a127-94332d2e5fe3")})
                .With(c => c.Id, secondConditionId.ToString())
                .Create();

            var dbCampaign = fixture.Create<CampaignDetails>();
            dbCampaign.Conditions = new List<Condition>()
            {
                fixture.Build<Condition>()
                    .With(c=>c.PartnerIds, oldPartnersIds)
                    .With(c=> c.Id, conditionId.ToString())
                    .Create(),
              conditionToBeDeleted
            };

            dbCampaign.Conditions.First().Id = conditionId.ToString("D");

            var updatingCampaign = fixture.Create<CampaignDetails>();
            updatingCampaign.Conditions = new List<Condition>()
            {
                fixture.Build<Condition>()
                    .With(c=>c.PartnerIds,
                        new List<Guid>()
                        {
                            Guid.Parse("9aae7587-1422-4986-a127-94332d2e5fe5")
                        })
                    .With(c=> c.Id, conditionId.ToString())
                    .Create(),
                fixture.Build<Condition>()
                    .With(c=>c.PartnerIds,
                        new List<Guid>()
                        {
                            Guid.Parse("9aae7587-1422-4986-a127-94332d2e5fe3")
                        })
                    .With(c=> c.Id, string.Empty)
                    .Create()
            };

            var validationResult = new ValidationResult();

            var campaignRepositoryMock = new Mock<ICampaignRepository>();
            var campaignValidationServiceMock = new Mock<ICampaignValidationService>();

            var conditionServiceMock = new Mock<IConditionService>();

            campaignRepositoryMock
                .Setup(x => x.GetCampaignAsync(campaignId))
                    .Returns(Task.FromResult(dbCampaign));

            campaignValidationServiceMock
                .Setup(x => x.ValidateUpdate(updatingCampaign, dbCampaign))
                    .Returns(validationResult);

            var oneMinutePublisher = new Mock<IRabbitPublisher<CampaignChangeEvent>>();
            var configRepositoryMock = new Mock<IConfigurationRepository>();

            var service = GetCampaignServiceInstance(
                campaignRepositoryMock.Object,
                conditionServiceMock.Object,
                campaignValidationServiceMock.Object,
                oneMinutePublisher.Object,
                configRepositoryMock.Object);

            // Act
            await service.UpdateAsync(updatingCampaign);

            // Assert
            conditionServiceMock.Verify(c => c.DeleteAsync(new List<Condition>(){conditionToBeDeleted}), times: Times.Once);
            conditionServiceMock.Verify(c => c.DeleteConditionPartnersAsync(new List<Guid>() { conditionId }), times: Times.Once);

            campaignRepositoryMock.Verify(expression: x => x.UpdateAsync(updatingCampaign));

            oneMinutePublisher.Verify(
                c => c.PublishAsync(
                    It.Is<CampaignChangeEvent>(e => e.Action == Contract.Enums.ActionType.Edited)), Times.Once);
        }
    }
}
