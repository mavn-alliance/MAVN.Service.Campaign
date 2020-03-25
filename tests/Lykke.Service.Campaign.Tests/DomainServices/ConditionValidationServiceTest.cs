using System;
using System.Collections.Generic;
using Lykke.Service.Campaign.Domain.Models;
using Lykke.Service.Campaign.Domain.Services;
using Lykke.Service.Campaign.DomainServices.Services;
using Moq;
using Xunit;

namespace Lykke.Service.Campaign.Tests.DomainServices
{
    public class ConditionValidationServiceTest
    {
        // ValidateConditionsHaveValidOrEmptyIds
        [Fact]
        public void When_NewConditionsAndDbConditionsHaveSameIds_Expect_ValidResponse()
        {
            // Arrange
            var bonusTypeValidationServiceMock = new Mock<IBonusTypeValidationService>();
            var fixture = new ConditionValidationServiceTestFixture();

            var conditionValidationService = new ConditionValidationService(bonusTypeValidationServiceMock.Object);

            // Act
            var validationResult = conditionValidationService.ValidateConditionsHaveValidOrEmptyIds(fixture.Conditions, fixture.DbConditions);

            // Assert
            Assert.True(validationResult.IsValid);
            Assert.Empty(validationResult.ValidationMessages);
        }

        [Fact]
        public void When_NewConditionsHaveNewConditionWithEmptyId_Expect_ValidResponse()
        {
            // Arrange
            var bonusTypeValidationServiceMock = new Mock<IBonusTypeValidationService>();
            var fixture = new ConditionValidationServiceTestFixture();
            fixture.Conditions.Add(new Condition());

            var conditionValidationService = new ConditionValidationService(bonusTypeValidationServiceMock.Object);

            // Act
            var validationResult = conditionValidationService.ValidateConditionsHaveValidOrEmptyIds(fixture.Conditions, fixture.DbConditions);

            // Assert
            Assert.True(validationResult.IsValid);
            Assert.Empty(validationResult.ValidationMessages);
        }

        [Fact]
        public void When_NewConditionsHaveNonExistingId_Expect_InvalidResponse()
        {
            // Arrange
            var bonusTypeValidationServiceMock = new Mock<IBonusTypeValidationService>();
            var fixture = new ConditionValidationServiceTestFixture();
            fixture.Conditions.Add(new Condition
            {
                Id = Guid.NewGuid().ToString("D")
            });

            var conditionValidationService = new ConditionValidationService(bonusTypeValidationServiceMock.Object);

            // Act
            var validationResult = conditionValidationService.ValidateConditionsHaveValidOrEmptyIds(fixture.Conditions, fixture.DbConditions);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.ValidationMessages);
        }

        // ValidateConditionPropertiesAreNotChanged
        [Fact]
        public void When_ConditionPropertiesAreNotChanged_Expect_ValidResponse()
        {
            // Arrange
            var bonusTypeValidationServiceMock = new Mock<IBonusTypeValidationService>();
            var fixture = new ConditionValidationServiceTestFixture();

            var conditionValidationService = new ConditionValidationService(bonusTypeValidationServiceMock.Object);

            // Act
            var validationResult = conditionValidationService.ValidateConditionPropertiesAreNotChanged(fixture.Conditions[0], fixture.DbConditions[0]);

            // Assert
            Assert.True(validationResult.IsValid);
            Assert.Empty(validationResult.ValidationMessages);
        }

        [Fact]
        public void When_ConditionPropertyCompletionCountIsChanged_Expect_InvalidResponse()
        {
            // Arrange
            var bonusTypeValidationServiceMock = new Mock<IBonusTypeValidationService>();
            var fixture = new ConditionValidationServiceTestFixture();
            fixture.Conditions[0].CompletionCount++;

            var conditionValidationService = new ConditionValidationService(bonusTypeValidationServiceMock.Object);

            // Act
            var validationResult = conditionValidationService.ValidateConditionPropertiesAreNotChanged(fixture.Conditions[0], fixture.DbConditions[0]);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.ValidationMessages);
        }

        [Fact]
        public void When_ConditionPropertyImmediateRewardIsChanged_Expect_InvalidResponse()
        {
            // Arrange
            var bonusTypeValidationServiceMock = new Mock<IBonusTypeValidationService>();
            var fixture = new ConditionValidationServiceTestFixture();
            fixture.Conditions[0].ImmediateReward++;

            var conditionValidationService = new ConditionValidationService(bonusTypeValidationServiceMock.Object);

            // Act
            var validationResult = conditionValidationService.ValidateConditionPropertiesAreNotChanged(fixture.Conditions[0], fixture.DbConditions[0]);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.ValidationMessages);
        }

        [Fact]
        public void When_ConditionPropertyBonusTypeIsChanged_Expect_InvalidResponse()
        {
            // Arrange
            var bonusTypeValidationServiceMock = new Mock<IBonusTypeValidationService>();
            var fixture = new ConditionValidationServiceTestFixture();
            fixture.Conditions[0].BonusType.Type = "referral";

            var conditionValidationService = new ConditionValidationService(bonusTypeValidationServiceMock.Object);

            // Act
            var validationResult = conditionValidationService.ValidateConditionPropertiesAreNotChanged(fixture.Conditions[0], fixture.DbConditions[0]);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.ValidationMessages);
        }

        // ValidateConditionsAreNotChanged
        [Fact]
        public void When_NewConditionsAndDbConditionsAreSame_Expect_ValidResponse()
        {
            // Arrange
            var bonusTypeValidationServiceMock = new Mock<IBonusTypeValidationService>();
            var fixture = new ConditionValidationServiceTestFixture();

            var conditionValidationService = new ConditionValidationService(bonusTypeValidationServiceMock.Object);

            // Act
            var validationResult = conditionValidationService.ValidateConditionsAreNotChanged(fixture.Conditions, fixture.DbConditions);

            // Assert
            Assert.True(validationResult.IsValid);
            Assert.Empty(validationResult.ValidationMessages);
        }

        [Fact]
        public void When_NewConditionsHaveNewCondition_Expect_InvalidResponse()
        {
            // Arrange
            var bonusTypeValidationServiceMock = new Mock<IBonusTypeValidationService>();
            var fixture = new ConditionValidationServiceTestFixture();
            fixture.Conditions.Add(new Condition());

            var conditionValidationService = new ConditionValidationService(bonusTypeValidationServiceMock.Object);

            // Act
            var validationResult = conditionValidationService.ValidateConditionsAreNotChanged(fixture.Conditions, fixture.DbConditions);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.ValidationMessages);
        }

        [Fact]
        public void When_ConditionPartnersAreAdded_Expect_InvalidResponse()
        {
            // Arrange
            var newPartnersIds = new List<Guid>()
            {
                Guid.Parse("9aae7587-1422-4986-a127-94332d2e5fe8"),
                Guid.Parse("2ea16a40-b990-4844-ab6d-883c5abe4cd8")
            };

            var bonusTypeValidationServiceMock = new Mock<IBonusTypeValidationService>();
            var fixture = new ConditionValidationServiceTestFixture();
            fixture.Conditions[0].PartnerIds = newPartnersIds;

            var conditionValidationService = new ConditionValidationService(bonusTypeValidationServiceMock.Object);

            // Act
            var validationResult = conditionValidationService.ValidateConditionPropertiesAreNotChanged(fixture.Conditions[0], fixture.DbConditions[0]);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.ValidationMessages);
        }

        [Fact]
        public void When_ConditionPartnersAreRemoved_Expect_InvalidResponse()
        {
            // Arrange
            var oldPartnersIds = new List<Guid>()
            {
                Guid.Parse("9aae7587-1422-4986-a127-94332d2e5fe8"),
                Guid.Parse("2ea16a40-b990-4844-ab6d-883c5abe4cd8")
            };

            var bonusTypeValidationServiceMock = new Mock<IBonusTypeValidationService>();
            var fixture = new ConditionValidationServiceTestFixture();
            fixture.DbConditions[0].PartnerIds = oldPartnersIds;

            var conditionValidationService = new ConditionValidationService(bonusTypeValidationServiceMock.Object);

            // Act
            var validationResult = conditionValidationService.ValidateConditionPropertiesAreNotChanged(fixture.Conditions[0], fixture.DbConditions[0]);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.ValidationMessages);
        }

        [Fact]
        public void When_ConditionPartnersCountAreChanged_Expect_InvalidResponse()
        {
            // Arrange
            var oldPartnersIds = new List<Guid>()
            {
                Guid.Parse("9aae7587-1422-4986-a127-94332d2e5fe8"),
                Guid.Parse("2ea16a40-b990-4844-ab6d-883c5abe4cd8")
            };

            var newPartnersIds = new List<Guid>()
            {
                Guid.Parse("9aae7587-1422-4986-a127-94332d2e5fe8")
            };

            var bonusTypeValidationServiceMock = new Mock<IBonusTypeValidationService>();
            var fixture = new ConditionValidationServiceTestFixture();
            fixture.DbConditions[0].PartnerIds = oldPartnersIds;
            fixture.Conditions[0].PartnerIds = newPartnersIds;

            var conditionValidationService = new ConditionValidationService(bonusTypeValidationServiceMock.Object);

            // Act
            var validationResult = conditionValidationService.ValidateConditionPropertiesAreNotChanged(fixture.Conditions[0], fixture.DbConditions[0]);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.ValidationMessages);
        }

        [Fact]
        public void When_ConditionPartnersAreChanged_Expect_InvalidResponse()
        {
            // Arrange
            var oldPartnersIds = new List<Guid>()
            {
                Guid.Parse("9aae7587-1422-4986-a127-94332d2e5fe8"),
                Guid.Parse("2ea16a40-b990-4844-ab6d-883c5abe4cd8")
            };

            var newPartnersIds = new List<Guid>()
            {
                Guid.Parse("3f7e614b-e4ce-4d06-bc57-b802baaae9b3"),
                Guid.Parse("1b8fc88e-a7fb-480f-b3d7-e10d9cbf1126")
            };

            var bonusTypeValidationServiceMock = new Mock<IBonusTypeValidationService>();
            var fixture = new ConditionValidationServiceTestFixture();
            fixture.DbConditions[0].PartnerIds = oldPartnersIds;
            fixture.Conditions[0].PartnerIds = newPartnersIds;

            var conditionValidationService = new ConditionValidationService(bonusTypeValidationServiceMock.Object);

            // Act
            var validationResult = conditionValidationService.ValidateConditionPropertiesAreNotChanged(fixture.Conditions[0], fixture.DbConditions[0]);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.ValidationMessages);
        }

        [Fact]
        public void When_ConditionPartnersAreNotChanged_Expect_ValidResponse()
        {
            // Arrange
            var partnersIds = new List<Guid>()
            {
                Guid.Parse("9aae7587-1422-4986-a127-94332d2e5fe8"),
                Guid.Parse("2ea16a40-b990-4844-ab6d-883c5abe4cd8")
            };

            var bonusTypeValidationServiceMock = new Mock<IBonusTypeValidationService>();
            var fixture = new ConditionValidationServiceTestFixture();
            fixture.DbConditions[0].PartnerIds = partnersIds;
            fixture.Conditions[0].PartnerIds = partnersIds;

            var conditionValidationService = new ConditionValidationService(bonusTypeValidationServiceMock.Object);

            // Act
            var validationResult = conditionValidationService.ValidateConditionPropertiesAreNotChanged(fixture.Conditions[0], fixture.DbConditions[0]);

            // Assert
            Assert.True(validationResult.IsValid);
            Assert.Empty(validationResult.ValidationMessages);
        }

        [Fact]
        public void When_OnePartnerIsAssignedMoreThanOnceToCondition_Expect_InvalidResponse()
        {
            // Arrange
            var partnersIds = new List<Guid>()
            {
                Guid.Parse("9aae7587-1422-4986-a127-94332d2e5fe8"),
                Guid.Parse("2ea16a40-b990-4844-ab6d-883c5abe4cd8"),
                Guid.Parse("2ea16a40-b990-4844-ab6d-883c5abe4cd8")
            };

            var bonusTypeValidationServiceMock = new Mock<IBonusTypeValidationService>();
            var fixture = new ConditionValidationServiceTestFixture();
            fixture.Conditions[0].PartnerIds = partnersIds;

            var conditionValidationService = new ConditionValidationService(bonusTypeValidationServiceMock.Object);

            // Act
            var validationResult = conditionValidationService.ValidateConditionsPartnersIds(new List<Condition>() { fixture.Conditions[0] });

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.NotEmpty(validationResult.ValidationMessages);
        }

        [Fact]
        public void When_OnePartnerIsAssignedOnlyOnceToCondition_Expect_ValidResponse()
        {
            // Arrange
            var partnersIds = new List<Guid>()
            {
                Guid.Parse("9aae7587-1422-4986-a127-94332d2e5fe8"),
                Guid.Parse("2ea16a40-b990-4844-ab6d-883c5abe4cd8")
            };

            var bonusTypeValidationServiceMock = new Mock<IBonusTypeValidationService>();
            var fixture = new ConditionValidationServiceTestFixture();
            fixture.Conditions[0].PartnerIds = partnersIds;

            var conditionValidationService = new ConditionValidationService(bonusTypeValidationServiceMock.Object);

            // Act
            var validationResult = conditionValidationService.ValidateConditionsPartnersIds(new List<Condition>() { fixture.Conditions[0] });

            // Assert
            Assert.True(validationResult.IsValid);
            Assert.Empty(validationResult.ValidationMessages);
        }

        [Fact]
        public void When_OnePartnersAreNotAssignedToCondition_Expect_ValidResponse()
        {
            // Arrange
            var partnersIds = new List<Guid>();

            var bonusTypeValidationServiceMock = new Mock<IBonusTypeValidationService>();
            var fixture = new ConditionValidationServiceTestFixture();
            fixture.Conditions[0].PartnerIds = partnersIds;

            var conditionValidationService = new ConditionValidationService(bonusTypeValidationServiceMock.Object);

            // Act
            var validationResult = conditionValidationService.ValidateConditionsPartnersIds(new List<Condition>() { fixture.Conditions[0] });

            // Assert
            Assert.True(validationResult.IsValid);
            Assert.Empty(validationResult.ValidationMessages);
        }

        [Fact]
        public void When_ConditionRewardRatioAreNotChanged_Expect_ValidResponse()
        {
            // Arrange
            var ratio = new RewardRatioAttributeModel()
            {
                Ratios = new List<RatioAttributeModel>()
                {
                    new RatioAttributeModel()
                    {
                        Order = 1,
                        RewardRatio = 20m,
                        PaymentRatio = 10m,
                        Threshold = 10m
                    },
                    new RatioAttributeModel()
                    {
                        Order = 2,
                        PaymentRatio = 10m,
                        RewardRatio = 20m,
                        Threshold = 20m
                    },
                    new RatioAttributeModel()
                    {
                        Order = 3,
                        PaymentRatio = 70m,
                        RewardRatio = 70m,
                        Threshold = 100m
                    }
                }

            };

            var bonusTypeValidationServiceMock = new Mock<IBonusTypeValidationService>();
            var fixture = new ConditionValidationServiceTestFixture();
            fixture.DbConditions[0].RewardRatio = ratio;
            fixture.Conditions[0].RewardRatio = ratio;

            var conditionValidationService = new ConditionValidationService(bonusTypeValidationServiceMock.Object);

            // Act
            var validationResult = conditionValidationService.ValidateConditionPropertiesAreNotChanged(fixture.Conditions[0], fixture.DbConditions[0]);

            // Assert
            Assert.True(validationResult.IsValid);
            Assert.Empty(validationResult.ValidationMessages);
        }

        [Fact]
        public void When_ConditionRewardRatioNewOneChangedToNull_Expect_NotValidResponse()
        {
            // Arrange
            var ratio = new RewardRatioAttributeModel()
            {
                Ratios = new List<RatioAttributeModel>()
                {
                    new RatioAttributeModel()
                    {
                        Order = 1,
                        RewardRatio = 20m,
                        PaymentRatio = 10m,
                        Threshold = 10m
                    },
                    new RatioAttributeModel()
                    {
                        Order = 2,
                        PaymentRatio = 10m,
                        RewardRatio = 20m,
                        Threshold = 20m
                    },
                    new RatioAttributeModel()
                    {
                        Order = 3,
                        PaymentRatio = 70m,
                        RewardRatio = 70m,
                        Threshold = 100m
                    }
                }
            };

            var bonusTypeValidationServiceMock = new Mock<IBonusTypeValidationService>();
            var fixture = new ConditionValidationServiceTestFixture();
            fixture.DbConditions[0].RewardRatio = ratio;
            fixture.Conditions[0].RewardRatio = null;

            var conditionValidationService = new ConditionValidationService(bonusTypeValidationServiceMock.Object);

            // Act
            var validationResult = conditionValidationService.ValidateConditionPropertiesAreNotChanged(fixture.Conditions[0], fixture.DbConditions[0]);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.NotEmpty(validationResult.ValidationMessages);
        }

        [Fact]
        public void When_ConditionRewardRatioNewOneChanged_Expect_NotValidResponse()
        {
            // Arrange
            var ratioOld = new RewardRatioAttributeModel()
            {
                Ratios = new List<RatioAttributeModel>()
                {
                    new RatioAttributeModel()
                    {
                        Order = 1,
                        RewardRatio = 20m,
                        PaymentRatio = 10m,
                        Threshold = 10m
                    },
                    new RatioAttributeModel()
                    {
                        Order = 2,
                        PaymentRatio = 10m,
                        RewardRatio = 20m,
                        Threshold = 20m
                    },
                    new RatioAttributeModel()
                    {
                        Order = 3,
                        PaymentRatio = 70m,
                        RewardRatio = 70m,
                        Threshold = 100m
                    }
                }
            };

            var ratioNew = new RewardRatioAttributeModel()
            {
                Ratios = new List<RatioAttributeModel>()
                {
                    new RatioAttributeModel()
                    {
                        Order = 1,
                        PaymentRatio = 10m,
                        RewardRatio = 10m,
                        Threshold = 10m
                    },
                    new RatioAttributeModel()
                    {
                        Order = 2,
                        RewardRatio = 20m,
                        PaymentRatio = 20m,
                        Threshold = 20m
                    },
                    new RatioAttributeModel()
                    {
                        Order = 3,
                        PaymentRatio = 70m,
                        RewardRatio = 70m,
                        Threshold = 100m
                    }
                }
            };

            var bonusTypeValidationServiceMock = new Mock<IBonusTypeValidationService>();
            var fixture = new ConditionValidationServiceTestFixture();
            fixture.DbConditions[0].RewardRatio = ratioOld;
            fixture.Conditions[0].RewardRatio = ratioNew;

            var conditionValidationService = new ConditionValidationService(bonusTypeValidationServiceMock.Object);

            // Act
            var validationResult = conditionValidationService.ValidateConditionPropertiesAreNotChanged(fixture.Conditions[0], fixture.DbConditions[0]);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.NotEmpty(validationResult.ValidationMessages);
        }
    }
}
