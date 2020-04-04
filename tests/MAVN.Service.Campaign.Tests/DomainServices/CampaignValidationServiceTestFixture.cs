using AutoFixture;
using MAVN.Service.Campaign.Domain.Enums;
using MAVN.Service.Campaign.Domain.Models;
using MAVN.Service.Campaign.Domain.Services;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using MAVN.Service.Campaign.Domain.Models.EarnRules;

namespace MAVN.Service.Campaign.Tests.DomainServices
{
    public class CampaignValidationServiceTestFixture
    {
        public CampaignDetails DbCampaign { get; set; }
        public CampaignDetails Campaign { get; set; }
        public Mock<IConditionValidationService> ConditionValidationServiceMock { get; set; }
        public Mock<IRuleContentValidationService> EarnRuleContentValidationService { get; set; }

        public CampaignValidationServiceTestFixture(CampaignStatus campaignStatus)
        {
            var fixture = new Fixture();
            DbCampaign = fixture.Create<CampaignDetails>();
            ConditionValidationServiceMock = new Mock<IConditionValidationService>();

            EarnRuleContentValidationService = new Mock<IRuleContentValidationService>();

            ConditionValidationServiceMock
                .Setup(c => c.ValidateConditionsBonusTypes(It.IsAny<IReadOnlyList<Condition>>()))
                .Returns(new ValidationResult());

            EarnRuleContentValidationService
                .Setup(c => c.ValidateHaveInvalidOrEmptyIds(It.IsAny<IReadOnlyList<Guid>>(), It.IsAny<IReadOnlyList<Guid>>()))
                .Returns(new ValidationResult());

            switch (campaignStatus)
            {
                case CampaignStatus.Pending:
                    {
                        DbCampaign.Conditions = fixture.CreateMany<Condition>(2).ToList();
                        DbCampaign.FromDate = DateTime.UtcNow.AddDays(2);
                        DbCampaign.ToDate = DateTime.UtcNow.AddDays(5);
                        DbCampaign.IsEnabled = true;
                    }
                    break;
                case CampaignStatus.Active:
                    {
                        DbCampaign.Conditions = fixture.CreateMany<Condition>(2).ToList();
                        DbCampaign.FromDate = DateTime.UtcNow.AddDays(-5);
                        DbCampaign.ToDate = DateTime.UtcNow.AddDays(5);
                        DbCampaign.IsEnabled = true;
                    }
                    break;
                case CampaignStatus.Completed:
                    {
                        DbCampaign.Conditions = fixture.CreateMany<Condition>(2).ToList();
                        DbCampaign.FromDate = DateTime.UtcNow.AddDays(-5);
                        DbCampaign.ToDate = DateTime.UtcNow.AddDays(-2);
                        DbCampaign.IsEnabled = true;
                    }
                    break;
                case CampaignStatus.Inactive:
                    {
                        DbCampaign.Conditions = fixture.CreateMany<Condition>(2).ToList();
                        DbCampaign.FromDate = DateTime.UtcNow.AddDays(-5);
                        DbCampaign.ToDate = DateTime.UtcNow.AddDays(5);
                        DbCampaign.IsEnabled = false;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(campaignStatus), campaignStatus, null);
            }

            Campaign = DeepClone(DbCampaign);
        }

        public void SetupConditionValidationServiceMockValidateConditionsAreNotChangedReturnsValidResult()
        {
            ConditionValidationServiceMock
                .Setup(c => c.ValidateConditionsAreNotChanged(
                    It.IsAny<IReadOnlyList<Condition>>(),
                    It.IsAny<IReadOnlyList<Condition>>()))
                .Returns(new ValidationResult());
        }

        public void SetupConditionValidationServiceMockValidateConditionsAreNotChangedReturnsInvalidResult()
        {
            ConditionValidationServiceMock
                .Setup(c => c.ValidateConditionsAreNotChanged(
                    It.IsAny<IReadOnlyList<Condition>>(),
                    It.IsAny<IReadOnlyList<Condition>>()))
                .Returns(new ValidationResult
                {
                    ValidationMessages = new List<string> { "ErrorMessage" }
                });
        }

        public void SetupConditionValidationServiceMockValidateConditionsHaveValidOrEmptyIdsReturnsValidResult()
        {
            ConditionValidationServiceMock
                .Setup(c => c.ValidateConditionsHaveValidOrEmptyIds(
                    It.IsAny<IReadOnlyList<Condition>>(),
                    It.IsAny<IReadOnlyList<Condition>>()))
                .Returns(new ValidationResult());
        }

        public void SetupConditionValidationServiceMockValidateConditionsPartnersReturnsValidResult()
        {
            ConditionValidationServiceMock
                .Setup(c => c.ValidateConditionsPartnersIds(
                It.IsAny<IReadOnlyList<Condition>>()))
                .Returns(new ValidationResult());
        }

        public void SetupConditionValidationServiceMockValidateConditionsPartnersReturnsInvalidResult()
        {
            ConditionValidationServiceMock
                .Setup(c => c.ValidateConditionsPartnersIds(
                    It.IsAny<IReadOnlyList<Condition>>()))
                .Returns(new ValidationResult
                {
                    ValidationMessages = new List<string> { "ErrorMessage" }
                });
        }

        public void SetupConditionValidationServiceMockValidateConditionsHaveValidOrEmptyIdsReturnsInvalidResult()
        {
            ConditionValidationServiceMock
                .Setup(c => c.ValidateConditionsHaveValidOrEmptyIds(
                    It.IsAny<IReadOnlyList<Condition>>(),
                    It.IsAny<IReadOnlyList<Condition>>()))
                .Returns(new ValidationResult
                {
                    ValidationMessages = new List<string> { "ErrorMessage" }
                });
        }

        private static T DeepClone<T>(T source)
        {
            if (ReferenceEquals(source, null))
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source));
        }
    }
}
