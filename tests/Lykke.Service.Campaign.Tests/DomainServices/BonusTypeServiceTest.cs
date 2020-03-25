using System.Threading.Tasks;
using AutoFixture;
using Lykke.Logs;
using Lykke.Service.Campaign.Domain.Exceptions;
using Lykke.Service.Campaign.Domain.Models;
using Lykke.Service.Campaign.Domain.Repositories;
using Lykke.Service.Campaign.DomainServices.Services;
using Moq;
using Xunit;

namespace Lykke.Service.Campaign.Tests.DomainServices
{
    public class BonusTypeServiceTest
    {
        private BonusTypeService GetBonusTypeService(IBonusTypeRepository bonusTypeRepository)
        {
            return new BonusTypeService(bonusTypeRepository, EmptyLogFactory.Instance);
        }

        // InsertAsync Tests
        [Fact]
        public async Task Should_CallBonusTypeRepositoryInsertAsync_When_CallingInsertAsync()
        {
            // Arrange
            var fixture = new Fixture();
            var bonusType = fixture.Create<BonusType>();
            var bonusTypeRepositoryMock = new Mock<IBonusTypeRepository>();
            bonusTypeRepositoryMock.Setup(x => x.GetBonusTypeAsync(bonusType.Type))
                .Returns(Task.FromResult<BonusType>(null));
            bonusTypeRepositoryMock.Setup(x => x.GetBonusTypeByDisplayNameAsync(bonusType.DisplayName))
                .Returns(Task.FromResult<BonusType>(null));

            var service = GetBonusTypeService(bonusTypeRepositoryMock.Object);

            // Act
            await service.InsertAsync(bonusType);

            // Assert
            bonusTypeRepositoryMock.Verify(x => x.InsertAsync(It.Is<BonusType>(b =>
                b.Type == bonusType.Type.ToLower())));
        }

        [Fact]
        public async Task Should_ThrowEntityAlreadyExistsException_When_CallingInsertAsyncWithExistingType()
        {
            // Arrange
            var fixture = new Fixture();
            var bonusType = fixture.Create<BonusType>();
            var bonusTypeRepositoryMock = new Mock<IBonusTypeRepository>();
            bonusTypeRepositoryMock.Setup(x => x.GetBonusTypeAsync(bonusType.Type.ToLower()))
                .ReturnsAsync(new BonusType());

            var service = GetBonusTypeService(bonusTypeRepositoryMock.Object);

            // Act
            // Assert
            await Assert.ThrowsAsync<EntityAlreadyExistsException>(() => service.InsertAsync(bonusType));
        }

        [Fact]
        public async Task Should_ThrowEntityAlreadyExistsException_When_CallingInsertAsyncWithExistingDisplayName()
        {
            // Arrange
            var fixture = new Fixture();
            var bonusType = fixture.Create<BonusType>();
            var bonusTypeRepositoryMock = new Mock<IBonusTypeRepository>();
            bonusTypeRepositoryMock.Setup(x => x.GetBonusTypeAsync(bonusType.Type.ToLower()))
                .Returns(Task.FromResult<BonusType>(null));
            bonusTypeRepositoryMock.Setup(x => x.GetBonusTypeByDisplayNameAsync(bonusType.DisplayName))
                .ReturnsAsync(new BonusType());

            var service = GetBonusTypeService(bonusTypeRepositoryMock.Object);

            // Act
            // Assert
            await Assert.ThrowsAsync<EntityAlreadyExistsException>(() => service.InsertAsync(bonusType));
        }

        //InsertIfNotExistsAsync
        [Fact]
        public async Task Should_NotCallInsertAsync_When_RepositoryGetBonusTypeAsyncReturnsObject()
        {
            // Arrange
            var fixture = new Fixture();
            var bonusType = fixture.Create<BonusType>();

            var bonusTypeRepositoryMock = new Mock<IBonusTypeRepository>();
            bonusTypeRepositoryMock
                .Setup(x => x.GetBonusTypeAsync(bonusType.Type.ToLower()))
                .ReturnsAsync(bonusType);

            var service = GetBonusTypeService(bonusTypeRepositoryMock.Object);

            // Act
            await service.InsertOrUpdateAsync(bonusType);

            // Assert
            bonusTypeRepositoryMock.Verify(x => x.InsertAsync(It.Is<BonusType>(b =>
                    b.Type == bonusType.Type.ToLower())),
                Times.Never);
        }

        [Fact]
        public async Task Should_CallInsertAsync_When_RepositoryGetBonusTypeAsyncDoesNotReturnObject()
        {
            // Arrange
            var fixture = new Fixture();
            var bonusType = fixture.Create<BonusType>();

            var bonusTypeRepositoryMock = new Mock<IBonusTypeRepository>();
            bonusTypeRepositoryMock
                .Setup(x => x.GetBonusTypeAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<BonusType>(null));

            var service = GetBonusTypeService(bonusTypeRepositoryMock.Object);

            // Act
            await service.InsertOrUpdateAsync(bonusType);

            // Assert
            bonusTypeRepositoryMock.Verify(x => x.InsertAsync(It.Is<BonusType>(b =>
                    b.Type == bonusType.Type.ToLower() &&
                    b.DisplayName == bonusType.DisplayName &&
                    b.IsAvailable == bonusType.IsAvailable)),
                Times.Once);
        }

        // UpdateAsync Tests
        [Fact]
        public async Task Should_CallBonusTypeRepositoryGetBonusTypeAsync_When_CallingUpdateAsync()
        {
            // Arrange
            var fixture = new Fixture();
            var bonusType = fixture.Create<BonusType>();

            var bonusTypeRepositoryMock = new Mock<IBonusTypeRepository>();
            bonusTypeRepositoryMock
                .Setup(x => x.GetBonusTypeAsync(bonusType.Type))
                .ReturnsAsync(new BonusType());

            var service = GetBonusTypeService(bonusTypeRepositoryMock.Object);

            // Act
            await service.UpdateAsync(bonusType);

            // Assert
            bonusTypeRepositoryMock.Verify(x => x.GetBonusTypeAsync(bonusType.Type));
        }

        [Fact]
        public async Task Should_ThrowEntityNotFoundException_When_CallingUpdateAsyncAndRepositoryReturnsNull()
        {
            // Arrange
            var fixture = new Fixture();

            var bonusType = fixture.Create<BonusType>();

            var bonusTypeRepositoryMock = new Mock<IBonusTypeRepository>();

            bonusTypeRepositoryMock
                .Setup(x => x.GetBonusTypeAsync(bonusType.Type))
                .Returns(Task.FromResult<BonusType>(null));

            var service = GetBonusTypeService(bonusTypeRepositoryMock.Object);

            // Act
            // Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(() => service.UpdateAsync(bonusType));
        }

        [Fact]
        public async Task Should_ThrowEntityAlreadyExistsException_When_CallingUpdateAsyncWithExistingDisplayName()
        {
            // Arrange
            var fixture = new Fixture();
            var bonusType = fixture.Create<BonusType>();

            var bonusTypeWithSameDisplayName = fixture.Create<BonusType>();
            bonusTypeWithSameDisplayName.DisplayName = bonusType.DisplayName;

            var bonusTypeRepositoryMock = new Mock<IBonusTypeRepository>();
            bonusTypeRepositoryMock.Setup(x => x.GetBonusTypeAsync(bonusType.Type))
                .ReturnsAsync(bonusType);
            bonusTypeRepositoryMock.Setup(x => x.GetBonusTypeByDisplayNameAsync(bonusType.DisplayName))
                .ReturnsAsync(bonusTypeWithSameDisplayName);

            var service = GetBonusTypeService(bonusTypeRepositoryMock.Object);

            // Act
            // Assert
            await Assert.ThrowsAsync<EntityAlreadyExistsException>(() => service.UpdateAsync(bonusType));
        }
    }
}
