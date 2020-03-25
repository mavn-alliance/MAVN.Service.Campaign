using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using Lykke.Service.Campaign.Domain.Models;
using Lykke.Service.Campaign.Domain.Repositories;
using Lykke.Service.Campaign.DomainServices.Services;
using Moq;
using Xunit;

namespace Lykke.Service.Campaign.Tests.DomainServices
{
    public class ConditionServiceTest
    {
        // InsertAsync
        [Fact]
        public async Task Should_CallRepositoryInsertAsync_WhenCallingInsertAsync()
        {
            // Arrange
            var fixture = new Fixture();
            var condition = fixture.Create<Condition>();

            var conditionRepositoryMock = new Mock<IConditionRepository>();
            var service = new ConditionService(conditionRepositoryMock.Object);

            // Act
            await service.InsertAsync(condition);

            // Assert
            conditionRepositoryMock.Verify(x => x.InsertAsync(condition));
        }

        // GetConditionsAsync
        [Fact]
        public async Task Should_CallRepositoryGetConditionsAsync_WhenCallingGetConditionsAsync()
        {
            // Arrange
            var conditionRepositoryMock = new Mock<IConditionRepository>();
            var service = new ConditionService(conditionRepositoryMock.Object);

            // Act
            await service.GetConditionsAsync();

            // Assert
            conditionRepositoryMock.Verify(x => x.GetConditionsAsync());
        }

        // GetConditionsByCampaignIdAsync
        [Fact]
        public async Task Should_CallRepositoryGetConditionsByCampaignIdAsync_WhenCallingGetConditionsByCampaignIdAsync()
        {
            // Arrange
            var id = Guid.NewGuid();
            var stringId = id.ToString("D");
            var conditionRepositoryMock = new Mock<IConditionRepository>();
            var service = new ConditionService(conditionRepositoryMock.Object);

            // Act
            await service.GetConditionsByCampaignIdAsync(stringId);

            // Assert
            conditionRepositoryMock.Verify(x => x.GetConditionsByCampaignIdAsync(id));
        }

        [Fact]
        public async Task ShouldThrowArgumentException_WhenCallingGetConditionsByCampaignIdAsyncWithNotGuidId()
        {
            // Arrange
            var fixture = new Fixture();
            var id = fixture.Create<string>().Substring(0, 10);
            var conditionRepositoryMock = new Mock<IConditionRepository>();
            var service = new ConditionService(conditionRepositoryMock.Object);

            // Act
            // Assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.GetConditionsByCampaignIdAsync(id));
        }

        [Fact]
        public async Task ShouldNotThrowArgumentException_WhenCallingGetConditionsByCampaignIdAsyncWithGuidId()
        {
            // Arrange
            var id = Guid.NewGuid().ToString("D");

            var conditionRepositoryMock = new Mock<IConditionRepository>();
            conditionRepositoryMock
                .Setup(x => x.DeleteAsync(It.IsAny<Condition>()))
                .Returns(Task.CompletedTask);

            var service = new ConditionService(conditionRepositoryMock.Object);

            // Act
            await service.GetConditionsByCampaignIdAsync(id);

            // Assert
            // Test will fail on exception
        }

        [Fact]
        public async Task ShouldGetAllConditionForType_WhenConditionTypeIsPassed()
        {
            // Arrange
            var bonusTypeSignUp = "SignUp";
            var conditionRepositoryMock = new Mock<IConditionRepository>(MockBehavior.Strict);

            conditionRepositoryMock
                .Setup(c => c.GetConditionsForConditionTypeAsync(bonusTypeSignUp, true))
                .ReturnsAsync(new List<Condition>().AsReadOnly());
            var service = new ConditionService(conditionRepositoryMock.Object);

            // Act
            await service.GetConditionsForConditionTypeAsync(bonusTypeSignUp, true);

            // Assert
            conditionRepositoryMock.Verify(c => c.GetConditionsForConditionTypeAsync(bonusTypeSignUp, true), Times.Once);
        }
    }
}
