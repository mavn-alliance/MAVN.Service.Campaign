using System;
using System.Collections.Generic;
using Lykke.Service.Campaign.Domain.Models;

namespace Lykke.Service.Campaign.Tests.DomainServices
{
    public class ConditionValidationServiceTestFixture
    {
        public List<Condition> Conditions { get; set; }
        public List<Condition> DbConditions { get; set; }

        public ConditionValidationServiceTestFixture()
        {
            var campaignId = Guid.NewGuid().ToString("D");
            var conditionId = Guid.NewGuid().ToString("D");
            Conditions = new List<Condition>
            {
                new Condition
                {
                    BonusType = new BonusType
                    {
                        Type = "SignUp",
                        DisplayName = "Sign Up",
                        CreationDate = DateTime.UtcNow,
                        IsAvailable = true
                    },
                    Id = conditionId,
                    CampaignId = campaignId,
                    ImmediateReward = 1,
                    CompletionCount = 1
                }
            };

            DbConditions = new List<Condition>
            {
                new Condition
                {
                    BonusType = new BonusType
                    {
                        Type = "SignUp",
                        DisplayName = "Sign Up",
                        CreationDate = DateTime.UtcNow,
                        IsAvailable = true
                    },
                    Id = conditionId,
                    CampaignId = campaignId,
                    ImmediateReward = 1,
                    CompletionCount = 1
                }
            };
        }
    }
}
