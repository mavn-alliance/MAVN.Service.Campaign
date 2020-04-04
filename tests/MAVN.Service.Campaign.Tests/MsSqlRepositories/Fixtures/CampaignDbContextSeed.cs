using System;
using System.Collections.Generic;
using MAVN.Service.Campaign.MsSqlRepositories;
using MAVN.Service.Campaign.MsSqlRepositories.Entities;
using Microsoft.EntityFrameworkCore.Internal;

namespace MAVN.Service.Campaign.Tests.MsSqlRepositories.Fixtures
{
    public class CampaignDbContextSeed
    {
        public const string BonusTypeSignUp = "signup";
        public static List<CampaignEntity> CampaignEntities { get; set; }

        public static Guid DeletedCampaignId = Guid.Parse("b0e04186-c95c-45b3-a9d9-ea01ca86fbd8");
        public static Guid NotEnabledCampaignId = Guid.Parse("372d7150-9bbb-4584-9bf9-72876f022163");
        public static Guid ActiveCampaignId = Guid.Parse("5f276d74-e561-4335-9dcb-30efe7b34f05");

        public static void Seed(CampaignContext context)
        {
            var today = DateTime.UtcNow;
            var creationDate = DateTime.UtcNow.AddMonths(-1);

            if (!context.Campaigns.Any())
            {
                CampaignEntities = new List<CampaignEntity>
                {
                    new CampaignEntity
                    {
                        Id = DeletedCampaignId,
                        Name = "DeletedCampaign",
                        Reward = 1500,
                        CreationDate = creationDate,
                        CompletionCount = 1,
                        IsDeleted = true,
                        IsEnabled = true,
                        FromDate = creationDate,
                        ToDate = today.AddDays(2),
                        ConditionEntities = new List<ConditionEntity>
                        {
                            new ConditionEntity
                            {
                                BonusTypeName = BonusTypeSignUp
                            }
                        }
                    },
                    new CampaignEntity
                    {
                        Id = ActiveCampaignId,
                        Name = "ActiveCampaign",
                        Reward = 1500,
                        CreationDate = creationDate.AddDays(1),
                        FromDate = today,
                        ToDate = today.AddDays(2),
                        CompletionCount = 1,
                        IsDeleted = false,
                        IsEnabled = true,
                        ConditionEntities = new List<ConditionEntity>
                        {
                            new ConditionEntity
                            {
                                BonusTypeName = BonusTypeSignUp
                            }
                        }
                    },
                    new CampaignEntity
                    {
                        Name = "ActiveWithoutEndDate",
                        Reward = 1500,
                        CreationDate = creationDate,
                        FromDate = today,
                        CompletionCount = 1,
                        IsDeleted = false,
                        IsEnabled = true
                    },
                    new CampaignEntity
                    {
                        Name = "PendingCampaign",
                        Reward = 1500,
                        CreationDate = creationDate,
                        FromDate = today.AddDays(10),
                        ToDate = today.AddDays(15),
                        CompletionCount = 1,
                        IsDeleted = false,
                        IsEnabled = true
                    },
                    new CampaignEntity
                    {
                        Id = NotEnabledCampaignId,
                        Name = "NotEnabledCampaign",
                        Reward = 1500,
                        CreationDate = creationDate,
                        FromDate = creationDate,
                        CompletionCount = 1,
                        IsDeleted = false,
                        IsEnabled = false
                    },
                    new CampaignEntity
                    {
                        Name = "CompletedCampaign",
                        Reward = 1500,
                        CreationDate = creationDate,
                        FromDate = creationDate,
                        ToDate = creationDate.AddDays(2),
                        CompletionCount = 1,
                        IsDeleted = false,
                        IsEnabled = true
                    }
                };

                context.Campaigns.AddRange(CampaignEntities);
                context.SaveChangesAsync();
            }
        }
    }
}
