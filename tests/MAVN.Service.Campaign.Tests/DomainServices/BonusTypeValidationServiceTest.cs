using System.Linq;
using MAVN.Service.Campaign.Domain.Services;
using MAVN.Service.Campaign.DomainServices.Services;
using Moq;
using Xunit;

namespace MAVN.Service.Campaign.Tests.DomainServices
{
    public class BonusTypeValidationServiceTest
    {
        #region ValidateBonusType
        [Fact]
        public void ValidateBonusType_WhenAValidBonusTypePassed_ReturnValidValidationResult()
        {
            //Arrange
            var bonusTypeServiceMock = new Mock<IBonusTypeService>();

            bonusTypeServiceMock.Setup(b => b.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(new Domain.Models.BonusType()
                {
                    IsAvailable = true
                });

            var bonusTypeValidationService = new BonusTypeValidationService(bonusTypeServiceMock.Object);

            //Act
            var result = bonusTypeValidationService.ValidateBonusType("type");

            //Assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public void ValidateBonusType_WhenANotValidBonusTypePassed_ReturnInValidValidationResult()
        {
            //Arrange
            var bonusTypeServiceMock = new Mock<IBonusTypeService>();

            bonusTypeServiceMock.Setup(b => b.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            var bonusTypeValidationService = new BonusTypeValidationService(bonusTypeServiceMock.Object);

            //Act
            var result = bonusTypeValidationService.ValidateBonusType("type");

            //Assert
            Assert.False(result.IsValid);
            Assert.Equal("Condition Type type is not a valid Type", result.ValidationMessages.First());
        }

        [Fact]
        public void ValidateBonusType_WhenANotAvailableBonusTypePassed_ReturnInValidValidationResult()
        {
            //Arrange
            var bonusTypeServiceMock = new Mock<IBonusTypeService>();

            bonusTypeServiceMock.Setup(b => b.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(new Domain.Models.BonusType()
                {
                    IsAvailable = false
                });

            var bonusTypeValidationService = new BonusTypeValidationService(bonusTypeServiceMock.Object);

            //Act
            var result = bonusTypeValidationService.ValidateBonusType("type");

            //Assert
            Assert.False(result.IsValid);
            Assert.Equal("Condition Type type is not available Type", result.ValidationMessages.First());
        }

        [Fact]
        public void ValidateBonusType_WhenANotStakebleBonusTypeForStackableConditionPassed_ReturnInValidValidationResult()
        {
            //Arrange
            var bonusTypeServiceMock = new Mock<IBonusTypeService>();

            var condition = new Domain.Models.Condition()
            {
                BonusType = new Domain.Models.BonusType
                {
                    Type = "type"
                },
                HasStaking = true
            };
            bonusTypeServiceMock.Setup(b => b.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(new Domain.Models.BonusType()
                {
                    IsAvailable = true,
                    IsStakeable = false
                });

            var bonusTypeValidationService = new BonusTypeValidationService(bonusTypeServiceMock.Object);

            //Act
            var result = bonusTypeValidationService.ValidateBonusType(condition.BonusType.Type, condition.HasStaking);

            //Assert
            Assert.False(result.IsValid);
            Assert.Equal("Condition Type type is not stakeable Type", result.ValidationMessages.First());
        }

        [Fact]
        public void ValidateBonusType_WhenStakebleBonusTypeForStackableConditionPassed_ReturnValidValidationResult()
        {
            //Arrange
            var bonusTypeServiceMock = new Mock<IBonusTypeService>();

            var condition = new Domain.Models.Condition()
            {
                BonusType = new Domain.Models.BonusType
                {
                    Type = "type"
                },
                HasStaking = true
            };
            bonusTypeServiceMock.Setup(b => b.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(new Domain.Models.BonusType()
                {
                    IsAvailable = true,
                    IsStakeable = true
                });

            var bonusTypeValidationService = new BonusTypeValidationService(bonusTypeServiceMock.Object);

            //Act
            var result = bonusTypeValidationService.ValidateBonusType(condition.BonusType.Type, condition.HasStaking);

            //Assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public void ValidateBonusType_WhenBonusTypeForNonStackableConditionPassed_ReturnValidValidationResult()
        {
            //Arrange
            var bonusTypeServiceMock = new Mock<IBonusTypeService>();

            var condition = new Domain.Models.Condition()
            {
                BonusType = new Domain.Models.BonusType
                {
                    Type = "type"
                },
                HasStaking = false
            };
            bonusTypeServiceMock.Setup(b => b.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(new Domain.Models.BonusType()
                {
                    IsAvailable = true,
                    IsStakeable = true
                });

            var bonusTypeValidationService = new BonusTypeValidationService(bonusTypeServiceMock.Object);

            //Act
            var result = bonusTypeValidationService.ValidateBonusType(condition.BonusType.Type, condition.HasStaking);

            //Assert
            Assert.True(result.IsValid);
        }
        #endregion
    }
}
