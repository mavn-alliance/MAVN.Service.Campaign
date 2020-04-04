using System;
using MAVN.Service.Campaign.Domain.Enums;
using MAVN.Service.Campaign.DomainServices.Services;
using Xunit;

namespace MAVN.Service.Campaign.Tests.DomainServices
{
    public class CampaignValidationServiceTest
    {
        // Note: Removed old earn rules validation on edit allowing modification of the earn rule at any time
        // Validating Active Campaign
        //[Fact]
        //public void When_ActiveCampaign_UpdatingRestrictedFields_ConditionValidationServiceReturnsValidResponse_Expect_InvalidResponse()
        //{
        //    // Arrange
        //    var fixture = new CampaignValidationServiceTestFixture(CampaignStatus.Active)
        //    {
        //        DbCampaign =
        //        {
        //            Reward = 3.0m
        //        },
        //        Campaign =
        //        {
        //            Reward = 4.0m
        //        }
        //    };

        //    fixture.SetupConditionValidationServiceMockValidateConditionsAreNotChangedReturnsValidResult();
        //    fixture.SetupConditionValidationServiceMockValidateConditionsPartnersReturnsValidResult();

        //    var campaignValidationService = new CampaignValidationService(fixture.ConditionValidationServiceMock.Object,
        //    fixture.EarnRuleContentValidationService.Object);

        //    // Act
        //    var validationResult = campaignValidationService.ValidateUpdate(fixture.Campaign, fixture.DbCampaign);

        //    // Assert
        //    Assert.False(validationResult.IsValid);
        //    Assert.Single(validationResult.ValidationMessages);
        //}

        [Fact]
        public void When_ActiveCampaign_UpdatingRestrictedFields_ConditionValidationServiceReturnsInvalidResponse_Expect_InvalidResponse()
        {
            // Arrange
            var fixture = new CampaignValidationServiceTestFixture(CampaignStatus.Active)
            {
                DbCampaign =
                {
                    Reward = 3.0m
                },
                Campaign =
                {
                    Reward = 4.0m
                }
            };

            fixture.SetupConditionValidationServiceMockValidateConditionsAreNotChangedReturnsInvalidResult();
            fixture.SetupConditionValidationServiceMockValidateConditionsPartnersReturnsInvalidResult();

            var campaignValidationService = new CampaignValidationService(fixture.ConditionValidationServiceMock.Object,
                fixture.EarnRuleContentValidationService.Object);
            // Act
            var validationResult = campaignValidationService.ValidateUpdate(fixture.Campaign, fixture.DbCampaign);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.ValidationMessages);
        }

        [Fact]
        public void When_ActiveCampaign_UpdatingNotRestrictedFields_ConditionValidationServiceReturnsValidResponse_Expect_ValidResponse()
        {
            // Arrange
            var fixture = new CampaignValidationServiceTestFixture(CampaignStatus.Active)
            {
                DbCampaign =
                {
                    Name = "dbCampaign",
                    Description = "dbCampaign"
                },
                Campaign =
                {
                    Name = "campaign",
                    Description = "campaign",
                    IsEnabled = false,
                    FromDate = DateTime.UtcNow.AddDays(-3),
                    ToDate = DateTime.UtcNow.AddDays(-1)
                }
            };

            fixture.SetupConditionValidationServiceMockValidateConditionsAreNotChangedReturnsValidResult();
            fixture.SetupConditionValidationServiceMockValidateConditionsPartnersReturnsValidResult();

            var campaignValidationService = new CampaignValidationService(fixture.ConditionValidationServiceMock.Object,
                fixture.EarnRuleContentValidationService.Object);
            // Act
            var validationResult = campaignValidationService.ValidateUpdate(fixture.Campaign, fixture.DbCampaign);

            // Assert
            Assert.True(validationResult.IsValid);
            Assert.Empty(validationResult.ValidationMessages);
        }

        [Fact]
        public void When_ActiveCampaign_UpdatingNotRestrictedFields_ConditionValidationServiceReturnsInvalidResponse_Expect_InvalidResponse()
        {
            // Arrange
            var fixture = new CampaignValidationServiceTestFixture(CampaignStatus.Active)
            {
                DbCampaign =
                {
                    Name = "dbCampaign",
                    Description = "dbCampaign"
                },
                Campaign =
                {
                    Name = "campaign",
                    Description = "campaign",
                    IsEnabled = false,
                    FromDate = DateTime.UtcNow.AddDays(-3),
                    ToDate = DateTime.UtcNow.AddDays(-1)
                }
            };

            fixture.SetupConditionValidationServiceMockValidateConditionsAreNotChangedReturnsInvalidResult();
            fixture.SetupConditionValidationServiceMockValidateConditionsPartnersReturnsInvalidResult();

            var campaignValidationService = new CampaignValidationService(fixture.ConditionValidationServiceMock.Object,
                fixture.EarnRuleContentValidationService.Object);
            // Act
            var validationResult = campaignValidationService.ValidateUpdate(fixture.Campaign, fixture.DbCampaign);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.ValidationMessages);
        }

        [Fact]
        public void When_ActiveCampaign_NoCampaignFieldsAreUpdated_ConditionValidationServiceReturnsValidResponse_Expect_InvalidResponse()
        {
            // Arrange
            var fixture = new CampaignValidationServiceTestFixture(CampaignStatus.Active);
            fixture.SetupConditionValidationServiceMockValidateConditionsAreNotChangedReturnsValidResult();
            fixture.SetupConditionValidationServiceMockValidateConditionsPartnersReturnsValidResult();

            var campaignValidationService = new CampaignValidationService(fixture.ConditionValidationServiceMock.Object,
                fixture.EarnRuleContentValidationService.Object);
            // Act
            var validationResult = campaignValidationService.ValidateUpdate(fixture.Campaign, fixture.DbCampaign);

            // Assert
            Assert.True(validationResult.IsValid);
            Assert.Empty(validationResult.ValidationMessages);
        }

        [Fact]
        public void When_ActiveCampaign_NoCampaignFieldsAreUpdated_ConditionValidationServiceReturnsInvalidResponse_Expect_InvalidResponse()
        {
            // Arrange
            var fixture = new CampaignValidationServiceTestFixture(CampaignStatus.Active);
            fixture.SetupConditionValidationServiceMockValidateConditionsAreNotChangedReturnsInvalidResult();
            fixture.SetupConditionValidationServiceMockValidateConditionsPartnersReturnsInvalidResult();

            var campaignValidationService = new CampaignValidationService(fixture.ConditionValidationServiceMock.Object,
                fixture.EarnRuleContentValidationService.Object);
            // Act
            var validationResult = campaignValidationService.ValidateUpdate(fixture.Campaign, fixture.DbCampaign);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.ValidationMessages);
        }

        // Note: Removed old earn rules validation on edit allowing modification of the earn rule at any time
        // Validating Inactive Campaign
        //[Fact]
        //public void When_InactiveCampaign_UpdatingRestrictedFields_ConditionValidationServiceReturnsValidResponse_Expect_InvalidResponse()
        //{
        //    // Arrange
        //    var fixture = new CampaignValidationServiceTestFixture(CampaignStatus.Inactive)
        //    {
        //        DbCampaign =
        //        {
        //            Name = "dbCampaign",
        //            Description = "dbCampaign",
        //            Reward = 3.0m
        //        },
        //        Campaign =
        //        {
        //            Name = "campaign",
        //            Description = "campaign",
        //            Reward = 4.0m,
        //        }
        //    };
        //    fixture.SetupConditionValidationServiceMockValidateConditionsAreNotChangedReturnsValidResult();
        //    fixture.SetupConditionValidationServiceMockValidateConditionsPartnersReturnsValidResult();

        //    var campaignValidationService = new CampaignValidationService(fixture.ConditionValidationServiceMock.Object,
        //        fixture.EarnRuleContentValidationService.Object);
        //    // Act
        //    var validationResult = campaignValidationService.ValidateUpdate(fixture.Campaign, fixture.DbCampaign);

        //    // Assert
        //    Assert.False(validationResult.IsValid);
        //    Assert.Equal(3, validationResult.ValidationMessages.Count());
        //}

        // Note: Removed old earn rules validation on edit allowing modification of the earn rule at any time
        //[Fact]
        //public void When_InactiveCampaign_UpdatingRestrictedFields_ConditionValidationServiceReturnsInvalidResponse_Expect_InvalidResponse()
        //{
        //    // Arrange
        //    var fixture = new CampaignValidationServiceTestFixture(CampaignStatus.Inactive)
        //    {
        //        DbCampaign =
        //        {
        //            Name = "dbCampaign",
        //            Description = "dbCampaign",
        //            Reward = 3.0m
        //        },
        //        Campaign =
        //        {
        //            Name = "campaign",
        //            Description = "campaign",
        //            Reward = 4.0m,
        //        }
        //    };
        //    fixture.SetupConditionValidationServiceMockValidateConditionsAreNotChangedReturnsInvalidResult();
        //    fixture.SetupConditionValidationServiceMockValidateConditionsPartnersReturnsValidResult();

        //    var campaignValidationService = new CampaignValidationService(fixture.ConditionValidationServiceMock.Object,
        //        fixture.EarnRuleContentValidationService.Object);
        //    // Act
        //    var validationResult = campaignValidationService.ValidateUpdate(fixture.Campaign, fixture.DbCampaign);

        //    // Assert
        //    Assert.False(validationResult.IsValid);
        //    Assert.Equal(4, validationResult.ValidationMessages.Count());
        //}

        [Fact]
        public void When_InactiveCampaign_UpdatingNotRestrictedFields_ConditionValidationServiceReturnsValidResponse_Expect_ValidResponse()
        {
            // Arrange
            var fixture = new CampaignValidationServiceTestFixture(CampaignStatus.Inactive)
            {
                Campaign =
                {
                    IsEnabled = false,
                    FromDate = DateTime.UtcNow.AddDays(-3),
                    ToDate = DateTime.UtcNow.AddDays(-1)
                }
            };
            fixture.SetupConditionValidationServiceMockValidateConditionsAreNotChangedReturnsValidResult();
            fixture.SetupConditionValidationServiceMockValidateConditionsPartnersReturnsValidResult();

            var campaignValidationService = new CampaignValidationService(fixture.ConditionValidationServiceMock.Object,
                fixture.EarnRuleContentValidationService.Object);
            // Act
            var validationResult = campaignValidationService.ValidateUpdate(fixture.Campaign, fixture.DbCampaign);

            // Assert
            Assert.True(validationResult.IsValid);
            Assert.Empty(validationResult.ValidationMessages);
        }

        // Note: Removed old earn rules validation on edit allowing modification of the earn rule at any time
        //[Fact]
        //public void When_InactiveCampaign_UpdatingNotRestrictedFields_ConditionValidationServiceReturnsInvalidResponse_Expect_InvalidResponse()
        //{
        //    // Arrange
        //    var fixture = new CampaignValidationServiceTestFixture(CampaignStatus.Inactive)
        //    {
        //        Campaign =
        //        {
        //            IsEnabled = false,
        //            FromDate = DateTime.UtcNow.AddDays(-3),
        //            ToDate = DateTime.UtcNow.AddDays(-1)
        //        }
        //    };
        //    fixture.SetupConditionValidationServiceMockValidateConditionsAreNotChangedReturnsInvalidResult();
        //    fixture.SetupConditionValidationServiceMockValidateConditionsPartnersReturnsValidResult();

        //    var campaignValidationService = new CampaignValidationService(fixture.ConditionValidationServiceMock.Object,
        //        fixture.EarnRuleContentValidationService.Object);
        //    // Act
        //    var validationResult = campaignValidationService.ValidateUpdate(fixture.Campaign, fixture.DbCampaign);

        //    // Assert
        //    Assert.False(validationResult.IsValid);
        //    Assert.Single(validationResult.ValidationMessages);
        //}

        // Note: Removed old earn rules validation on edit allowing modification of the earn rule at any time
        // Validating Completed Campaign
        //[Fact]
        //public void When_CompletedCampaign_UpdatingRestrictedFields_ConditionValidationServiceReturnsValidResponse_Expect_InvalidResponse()
        //{
        //    // Arrange
        //    var fixture = new CampaignValidationServiceTestFixture(CampaignStatus.Completed)
        //    {
        //        DbCampaign =
        //        {
        //            Name = "dbCampaign",
        //            Description = "dbCampaign",
        //            Reward = 3.0m
        //        },
        //        Campaign =
        //        {
        //            Reward = 4.0m,
        //            Name = "campaign",
        //            Description = "campaign",
        //            IsEnabled = false,
        //            FromDate = DateTime.UtcNow.AddDays(-3),
        //            ToDate = DateTime.UtcNow.AddDays(-1)
        //        }
        //    };
        //    fixture.SetupConditionValidationServiceMockValidateConditionsAreNotChangedReturnsValidResult();
        //    fixture.SetupConditionValidationServiceMockValidateConditionsPartnersReturnsValidResult();

        //    var campaignValidationService = new CampaignValidationService(fixture.ConditionValidationServiceMock.Object,
        //        fixture.EarnRuleContentValidationService.Object);
        //    // Act
        //    var validationResult = campaignValidationService.ValidateUpdate(fixture.Campaign, fixture.DbCampaign);

        //    // Assert
        //    Assert.False(validationResult.IsValid);
        //    Assert.Equal(6, validationResult.ValidationMessages.Count());
        //}

        // Note: Removed old earn rules validation on edit allowing modification of the earn rule at any time
        //[Fact]
        //public void When_CompletedCampaign_UpdatingRestrictedFields_ConditionValidationServiceReturnsInvalidResponse_Expect_InvalidResponse()
        //{
        //    // Arrange
        //    var fixture = new CampaignValidationServiceTestFixture(CampaignStatus.Completed)
        //    {
        //        DbCampaign =
        //        {
        //            Name = "dbCampaign",
        //            Description = "dbCampaign",
        //            Reward = 3.0m
        //        },
        //        Campaign =
        //        {
        //            Reward = 4.0m,
        //            Name = "campaign",
        //            Description = "campaign",
        //            IsEnabled = false,
        //            FromDate = DateTime.UtcNow.AddDays(-3),
        //            ToDate = DateTime.UtcNow.AddDays(-1)
        //        }
        //    };

        //    fixture.SetupConditionValidationServiceMockValidateConditionsAreNotChangedReturnsInvalidResult();
        //    fixture.SetupConditionValidationServiceMockValidateConditionsPartnersReturnsValidResult();

        //    var campaignValidationService = new CampaignValidationService(fixture.ConditionValidationServiceMock.Object,
        //        fixture.EarnRuleContentValidationService.Object);
        //    // Act
        //    var validationResult = campaignValidationService.ValidateUpdate(fixture.Campaign, fixture.DbCampaign);

        //    // Assert
        //    Assert.False(validationResult.IsValid);
        //    Assert.Equal(7, validationResult.ValidationMessages.Count());
        //}

        [Fact]
        public void When_CompletedCampaign_NoFieldsUpdated_ConditionValidationServiceReturnsValidResponse_Expect_ValidResponse()
        {
            // Arrange
            var fixture = new CampaignValidationServiceTestFixture(CampaignStatus.Completed);
            fixture.SetupConditionValidationServiceMockValidateConditionsAreNotChangedReturnsValidResult();
            fixture.SetupConditionValidationServiceMockValidateConditionsPartnersReturnsValidResult();

            var campaignValidationService = new CampaignValidationService(fixture.ConditionValidationServiceMock.Object,
                fixture.EarnRuleContentValidationService.Object);
            // Act
            var validationResult = campaignValidationService.ValidateUpdate(fixture.Campaign, fixture.DbCampaign);

            // Assert
            Assert.True(validationResult.IsValid);
            Assert.Empty(validationResult.ValidationMessages);
        }

        [Fact]
        public void When_CompletedCampaign_NoFieldsUpdated_ConditionValidationServiceReturnsInvalidResponse_Expect_InvalidResponse()
        {
            // Arrange
            var fixture = new CampaignValidationServiceTestFixture(CampaignStatus.Completed);
            fixture.SetupConditionValidationServiceMockValidateConditionsAreNotChangedReturnsInvalidResult();
            fixture.SetupConditionValidationServiceMockValidateConditionsPartnersReturnsInvalidResult();

            var campaignValidationService = new CampaignValidationService(fixture.ConditionValidationServiceMock.Object,
                fixture.EarnRuleContentValidationService.Object);
            // Act
            var validationResult = campaignValidationService.ValidateUpdate(fixture.Campaign, fixture.DbCampaign);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.ValidationMessages);
        }

        // Validating Pending Campaign
        [Fact]
        public void When_PendingCampaign_UpdatingAllFields_ConditionValidationServiceReturnsValidResponse_Expect_ValidResponse()
        {
            // Arrange
            var fixture = new CampaignValidationServiceTestFixture(CampaignStatus.Pending)
            {
                DbCampaign =
                {
                    Name = "dbCampaign",
                    Description = "dbCampaign",
                    Reward = 3.0m
                },
                Campaign =
                {
                    Reward = 4.0m,
                    Name = "campaign",
                    Description = "campaign",
                    IsEnabled = false,
                    FromDate = DateTime.UtcNow.AddDays(-3),
                    ToDate = DateTime.UtcNow.AddDays(-1)
                }
            };

            fixture.SetupConditionValidationServiceMockValidateConditionsHaveValidOrEmptyIdsReturnsValidResult();
            fixture.SetupConditionValidationServiceMockValidateConditionsPartnersReturnsValidResult();

            var campaignValidationService = new CampaignValidationService(fixture.ConditionValidationServiceMock.Object,
                fixture.EarnRuleContentValidationService.Object);
            // Act
            var validationResult = campaignValidationService.ValidateUpdate(fixture.Campaign, fixture.DbCampaign);

            // Assert
            Assert.True(validationResult.IsValid);
            Assert.Empty(validationResult.ValidationMessages);
        }

        // Note: Removed old earn rules validation on edit allowing modification of the earn rule at any time
        //[Fact]
        //public void When_PendingCampaign_UpdatingAllFields_ConditionValidationServiceReturnsInvalidResponse_Expect_InvalidResponse()
        //{
        //    // Arrange
        //    var fixture = new CampaignValidationServiceTestFixture(CampaignStatus.Pending)
        //    {
        //        DbCampaign =
        //        {
        //            Name = "dbCampaign",
        //            Description = "dbCampaign",
        //            Reward = 3.0m
        //        },
        //        Campaign =
        //        {
        //            Reward = 4.0m,
        //            Name = "campaign",
        //            Description = "campaign",
        //            IsEnabled = false,
        //            FromDate = DateTime.UtcNow.AddDays(-3),
        //            ToDate = DateTime.UtcNow.AddDays(-1)
        //        }
        //    };

        //    fixture.SetupConditionValidationServiceMockValidateConditionsHaveValidOrEmptyIdsReturnsInvalidResult();
        //    fixture.SetupConditionValidationServiceMockValidateConditionsPartnersReturnsValidResult();
        //    fixture.SetupConditionValidationServiceMockValidateConditionsPartnersReturnsValidResult();

        //    var campaignValidationService = new CampaignValidationService(fixture.ConditionValidationServiceMock.Object,
        //        fixture.EarnRuleContentValidationService.Object);
        //    // Act
        //    var validationResult = campaignValidationService.ValidateUpdate(fixture.Campaign, fixture.DbCampaign);

        //    // Assert
        //    Assert.False(validationResult.IsValid);
        //    Assert.Single(validationResult.ValidationMessages);
        //}
    }
}
