using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Common;
using Common.Log;
using Lykke.Common.Log;
using MAVN.Numerics;
using MAVN.Service.Campaign.Contract.Enums;
using MAVN.Service.Campaign.Contract.Events;
using MAVN.Service.Campaign.Domain.Enums;
using MAVN.Service.Campaign.Domain.Exceptions;
using MAVN.Service.Campaign.Domain.Extensions;
using MAVN.Service.Campaign.Domain.Models;
using MAVN.Service.Campaign.Domain.Models.EarnRules;
using MAVN.Service.Campaign.Domain.Repositories;
using MAVN.Service.Campaign.Domain.Services;
using MAVN.Service.Campaign.DomainServices.Helpers;
using Lykke.RabbitMqBroker.Publisher;
using EventCampaignStatus = MAVN.Service.Campaign.Contract.Enums.CampaignStatus;
using CampaignStatus = MAVN.Service.Campaign.Domain.Enums.CampaignStatus;

namespace MAVN.Service.Campaign.DomainServices.Services
{
    public class CampaignService : ICampaignService
    {
        private readonly ICampaignRepository _campaignRepository;
        private readonly IConfigurationRepository _configurationRepository;
        private readonly IRabbitPublisher<CampaignChangeEvent> _campaignChangeEventPublisher;
        private readonly IConditionService _conditionService;
        private readonly ICampaignValidationService _campaignValidationService;
        private readonly ILog _log;
        private readonly IFileService _fileService;
        private readonly IEarnRuleContentRepository _earnRuleContentRepository;
        private readonly IMapper _mapper;

        public CampaignService(
            ICampaignRepository campaignRepository,
            IConditionService conditionService,
            ICampaignValidationService campaignValidationService,
            ILogFactory logFactor,
            IRabbitPublisher<CampaignChangeEvent> campaignChangeEventPublisher,
            IConfigurationRepository configurationRepository,
            IFileService fileService,
            IEarnRuleContentRepository earnRuleContentRepository,
            IMapper mapper)
        {
            _campaignRepository = campaignRepository;
            _conditionService = conditionService;
            _campaignValidationService = campaignValidationService;
            _campaignChangeEventPublisher = campaignChangeEventPublisher;
            _configurationRepository = configurationRepository;
            _fileService = fileService;
            _earnRuleContentRepository = earnRuleContentRepository;
            _log = logFactor.CreateLog(this);
            _mapper = mapper;
        }

        public async Task<string> InsertAsync(CampaignDetails campaign)
        {
            campaign.CreationDate = DateTime.UtcNow;

            var response = _campaignValidationService.ValidateInsert(campaign);

            if (response != null && !response.IsValid)
            {
                throw new EntityNotValidException(string.Join(Environment.NewLine, response.ValidationMessages));
            }

            if (campaign.UsePartnerCurrencyRate)
            {
                campaign.AmountInTokens = null;
                campaign.AmountInCurrency = null;
            }

            foreach (var condition in campaign.Conditions)
            {
                if (condition.UsePartnerCurrencyRate)
                {
                    condition.AmountInTokens = null;
                    condition.AmountInCurrency = null;
                }

                if (condition.RewardRatio != null)
                    SetRewardThreshold(condition.RewardRatio);
            }

            var campaignId = await _campaignRepository.InsertAsync(campaign);

            _log.Info($"Campaign was added: {campaign.ToJson()}", process: nameof(InsertAsync), context: campaign.Id);

            await PublishCampaignChangeEvent(campaignId, _mapper.Map<CampaignStatus, EventCampaignStatus>(campaign.CampaignStatus), ActionType.Created);

            return campaignId.ToString("D");
        }

        public async Task UpdateAsync(CampaignDetails campaign)
        {
            var oldCampaign = await _campaignRepository.GetCampaignAsync(campaign.Id.ToGuid());

            if (oldCampaign == null)
            {
                throw new EntityNotFoundException($"Campaign with id {campaign.Id} does not exist.");
            }

            var response = _campaignValidationService.ValidateUpdate(campaign, oldCampaign);

            if (!response.IsValid)
            {
                throw new EntityNotValidException(string.Join(Environment.NewLine, response.ValidationMessages));
            }

            // copy old values to preserve them after update
            campaign.CreatedBy = oldCampaign.CreatedBy;
            campaign.CreationDate = oldCampaign.CreationDate;

            if (campaign.UsePartnerCurrencyRate)
            {
                campaign.AmountInTokens = null;
                campaign.AmountInCurrency = null;
            }

            var conditionsToRemove = oldCampaign.Conditions.Where(c1 => campaign.Conditions.All(c2 => c1.Id != c2.Id));
            await _conditionService.DeleteAsync(conditionsToRemove);

            var conditionsToBeChanged = oldCampaign.Conditions.Where(c1 => campaign.Conditions.Any(c2 => c1.Id == c2.Id)).ToList();

            foreach (var condition in conditionsToBeChanged)
            {
                if (condition.UsePartnerCurrencyRate)
                {
                    condition.AmountInTokens = null;
                    condition.AmountInCurrency = null;
                }
            }

            await _conditionService.DeleteConditionPartnersAsync(conditionsToBeChanged.Select(c => Guid.Parse(c.Id)));

            await _conditionService.DeleteConditionAttributesAsync(conditionsToBeChanged.Select(c => Guid.Parse(c.Id)));

            var contentsToRemove = oldCampaign.Contents.Where(c1 => campaign.Contents.All(c2 => c1.Id != c2.Id)).ToList();
            await _earnRuleContentRepository.DeleteAsync(contentsToRemove);

            foreach (var content in contentsToRemove)
            {
                if (content.RuleContentType == RuleContentType.UrlForPicture)
                {
                    await _fileService.DeleteAsync(content.Id);
                }
            }

            foreach (var condition in campaign.Conditions)
            {
                if (condition.RewardRatio != null)
                    SetRewardThreshold(condition.RewardRatio);
            }

            await _campaignRepository.UpdateAsync(campaign);

            await PublishCampaignChangeEvent(campaign.Id.ToGuid(),
                _mapper.Map<CampaignStatus, EventCampaignStatus>(campaign.CampaignStatus),
                ActionType.Edited);

            _log.Info($"Campaign was updated: {campaign.ToJson()}", process: nameof(UpdateAsync), context: campaign.Id);
        }

        private static void SetRewardThreshold(RewardRatioAttributeModel conditionRewardRatio)
        {
            var threshold = 0m;

            foreach (var ratio in conditionRewardRatio.Ratios.OrderBy(r => r.Order))
            {
                threshold += ratio.PaymentRatio;

                ratio.Threshold += threshold;
            }
        }

        public async Task DeleteAsync(string campaignId)
        {
            var campaign = await _campaignRepository.GetCampaignAsync(campaignId.ToGuid());

            if (campaign == null)
            {
                _log.Info($"Campaign with id {campaignId} does not exists.", campaignId, process: nameof(DeleteAsync));

                return;
            }

            await _campaignRepository.DeleteAsync(campaign);

            foreach (var content in campaign.Contents)
            {
                await _fileService.DeleteAsync(content.Id);
            }

            await PublishCampaignChangeEvent(campaignId.ToGuid(),
                _mapper.Map<CampaignStatus, EventCampaignStatus>(campaign.CampaignStatus),
                ActionType.Deleted);

            _log.Info("Campaign was deleted", campaignId, process: nameof(DeleteAsync));
        }

        public async Task<CampaignDetails> GetCampaignAsync(string campaignId)
        {
            var campaign = await _campaignRepository.GetCampaignAsync(campaignId.ToGuid());

            if (campaign == null)
            {
                throw new EntityNotFoundException($"Campaign with id {campaignId} does not exist.");
            }

            foreach (var content in campaign.Contents)
            {
                if (content.RuleContentType is RuleContentType.UrlForPicture)
                    content.Image = await _fileService.GetAsync(content.Id);
            }

            return campaign;
        }

        public async Task<CampaignDetails> GetByIdAsync(Guid campaignId)
        {
            var campaign = await _campaignRepository.GetByIdAsync(campaignId);

            if (campaign == null)
                throw new EntityNotFoundException($"Campaign with id {campaignId} does not exist.");

            foreach (var content in campaign.Contents)
            {
                if (content.RuleContentType is RuleContentType.UrlForPicture)
                    content.Image = await _fileService.GetAsync(content.Id);
            }

            return campaign;
        }

        public async Task<IReadOnlyCollection<CampaignDetails>> GetCampaignsAsync()
        {
            return await _campaignRepository.GetCampaignsAsync();
        }

        public async Task<IReadOnlyCollection<CampaignDetails>> GetActiveCampaignsAsync()
        {
            return await _campaignRepository.GetActiveCampaignsAsync();
        }

        public async Task<PaginatedCampaignListModel> GetPagedCampaignsAsync(CampaignListRequestModel campaignListRequestModel)
        {
            return await _campaignRepository.GetPagedCampaignsAsync(campaignListRequestModel);
        }

        public async Task<IReadOnlyCollection<CampaignDetails>> GetCampaignsByIdsAsync(Guid[] campaignsIds)
        {
            return await _campaignRepository.GetCampaignsByIdsAsync(campaignsIds);
        }

        public async Task<(bool isSuccessful, string errorMessage)> ProcessOneMinuteTimeEvent(DateTime now)
        {
            var configData = await _configurationRepository.Get();

            var lastProcessedDate = configData ?? default;

            var activeNow =
                await _campaignRepository.GetAllByStatus(CampaignStatus.Active, now);

            IReadOnlyList<CampaignDto> changedFromPendingToActive;

            if (lastProcessedDate != default)
            {
                var activeOnLastProcessedDate = await _campaignRepository
                     .GetAllByStatus(CampaignStatus.Active, lastProcessedDate);

                var pendingOnLastProcessedDate = await _campaignRepository
                    .GetAllByStatus(CampaignStatus.Pending, lastProcessedDate);

                var completedCampaigns = activeOnLastProcessedDate.Except(activeNow, new CampaignDtoEqualComparer());

                foreach (var completedOne in completedCampaigns)
                {
                    if (!completedOne.IsDeleted && completedOne.IsEnabled)
                    {
                        await PublishCampaignChangeEvent(
                        completedOne.Id,
                        EventCampaignStatus.Completed,
                        ActionType.Completed);
                    }
                }

                changedFromPendingToActive = pendingOnLastProcessedDate.Intersect(activeNow, new CampaignDtoEqualComparer()).ToList();
            }
            else
            {
                changedFromPendingToActive = activeNow;
            }

            foreach (var newActive in changedFromPendingToActive)
            {
                if (!newActive.IsDeleted && newActive.IsEnabled)
                {
                    await PublishCampaignChangeEvent(
                        newActive.Id,
                        EventCampaignStatus.Active,
                        ActionType.Activated);
                }
            }

            await _configurationRepository.Set(now);

            return (true, string.Empty);
        }

        public async Task SaveCampaignContentImage(FileModel file)
        {
            var earnRuleContent = await _earnRuleContentRepository.GetAsync(file.RuleContentId);

            if (earnRuleContent == null)
            {
                throw new EntityNotFoundException($"Earn Content type with id {file.RuleContentId} does not exist.");
            }

            if (earnRuleContent.RuleContentType is RuleContentType.UrlForPicture)
            {
                var url = await _fileService.SaveAsync(file);

                earnRuleContent.Value = url;

                await _earnRuleContentRepository.UpdateAsync(earnRuleContent);
            }
            else
            {
                throw new RuleConditionNotFileException($"Earn Content type with id {file.RuleContentId} is not image type");
            }
        }

        public async Task<PaginatedEarnRuleListModel> GetEarnRulesPagedAsync(
            Localization language,
            List<CampaignStatus> statuses,
            PaginationModel pagination)
        {
            var results = await _campaignRepository.GetEnabledCampaignsByStatusAsync(statuses, pagination);

            return new PaginatedEarnRuleListModel
            {
                TotalCount = results.TotalCount,
                CurrentPage = results.CurrentPage,
                PageSize = results.PageSize,
                EarnRules = results.Campaigns.Select(result => new EarnRuleLocalizedModel
                {
                    Id = result.Id,
                    Reward = CalculateTotalReward(result),
                    AmountInTokens = result.AmountInTokens,
                    AmountInCurrency = result.AmountInCurrency,
                    UsePartnerCurrencyRate = result.UsePartnerCurrencyRate,
                    RewardType = result.RewardType,
                    FromDate = result.FromDate,
                    ToDate = result.ToDate,
                    CreatedBy = result.CreatedBy,
                    CreationDate = result.CreationDate,
                    CompletionCount = result.CompletionCount,
                    ApproximateAward = result.ApproximateAward,
                    IsApproximate = result.RewardType == RewardType.Percentage
                        || result.RewardType == RewardType.ConversionRate
                        || (result.Conditions.Any(cc => cc.RewardType == RewardType.Percentage || cc.RewardType == RewardType.ConversionRate)),
                    Title = result.GetContent(RuleContentType.Title, language)?.Value,
                    Status = result.CampaignStatus,
                    Description = result.GetContent(RuleContentType.Description, language)?.Value,
                    ImageUrl = result.GetContent(RuleContentType.UrlForPicture, language)?.Value,
                    Order = result.Order,
                    Conditions = result.Conditions
                        .Select(c => new ConditionLocalizedModel
                        {
                            Id = c.Id,
                            Type = c.BonusType.Type,
                            Vertical = c.BonusType.Vertical,
                            IsHidden = c.BonusType.IsHidden,
                            DisplayName = c.BonusType.DisplayName,
                            ImmediateReward = c.ImmediateReward,
                            CompletionCount = c.CompletionCount,
                            PartnerIds = c.PartnerIds,
                            HasStaking = c.HasStaking,
                            StakeAmount = c.StakeAmount,
                            StakeWarningPeriod = c.StakeWarningPeriod,
                            StakingPeriod = c.StakingPeriod,
                            StakingRule = c.StakingRule,
                            BurningRule = c.BurningRule,
                            UsePartnerCurrencyRate = c.UsePartnerCurrencyRate,
                            RewardType = c.RewardType,
                            AmountInCurrency = c.AmountInCurrency,
                            AmountInTokens = c.AmountInTokens,
                            RewardRatio = c.RewardRatio,
                            ApproximateAward = c.ApproximateAward,
                            IsApproximate = c.RewardType == RewardType.Percentage || c.RewardType == RewardType.ConversionRate,
                        }).ToList()
                }).ToList()
            };
        }

        public async Task<EarnRuleLocalizedModel> GetAsync(Guid earnRuleId, Localization localization)
        {
            var result = await _campaignRepository.GetCampaignAsync(earnRuleId);

            if (result == null || result.IsDeleted || !result.IsEnabled)
                return null;

            var mobileResponse = BuildEarnRuleLocalizedModel(localization, result);

            return mobileResponse;
        }

        private static EarnRuleLocalizedModel BuildEarnRuleLocalizedModel(Localization localization, CampaignDetails result)
        {
            var mobileResponse = new EarnRuleLocalizedModel
            {
                Id = result.Id,
                Reward = CalculateTotalReward(result),
                AmountInTokens = result.AmountInTokens,
                AmountInCurrency = result.AmountInCurrency,
                UsePartnerCurrencyRate = result.UsePartnerCurrencyRate,
                RewardType = result.RewardType,
                FromDate = result.FromDate,
                ToDate = result.ToDate,
                CreatedBy = result.CreatedBy,
                CreationDate = result.CreationDate,
                CompletionCount = result.CompletionCount,
                Title = result.GetContent(RuleContentType.Title, localization)?.Value,
                Status = result.CampaignStatus,
                Description = result.GetContent(RuleContentType.Description, localization)?.Value,
                ImageUrl = result.GetContent(RuleContentType.UrlForPicture, localization)?.Value,
                ApproximateAward = result.ApproximateAward,
                Order = result.Order,
                IsApproximate = result.RewardType == RewardType.Percentage
                    || result.RewardType == RewardType.ConversionRate
                    || (result.Conditions.Any(cc => cc.RewardType == RewardType.Percentage || cc.RewardType == RewardType.ConversionRate)),

                Conditions = result.Conditions
                    .Select(c => new ConditionLocalizedModel
                    {
                        Id = c.Id,
                        Type = c.BonusType.Type,
                        IsHidden = c.BonusType.IsHidden,
                        DisplayName = c.BonusType.DisplayName,
                        ImmediateReward = c.ImmediateReward,
                        CompletionCount = c.CompletionCount,
                        PartnerIds = c.PartnerIds,
                        HasStaking = c.HasStaking,
                        StakeAmount = c.StakeAmount,
                        StakeWarningPeriod = c.StakeWarningPeriod,
                        StakingPeriod = c.StakingPeriod,
                        StakingRule = c.StakingRule,
                        BurningRule = c.BurningRule,
                        UsePartnerCurrencyRate = c.UsePartnerCurrencyRate,
                        RewardType = c.RewardType,
                        AmountInCurrency = c.AmountInCurrency,
                        AmountInTokens = c.AmountInTokens,
                        RewardRatio = c.RewardRatio,
                        ApproximateAward = c.ApproximateAward,
                        IsApproximate = c.RewardType == RewardType.Percentage || c.RewardType == RewardType.ConversionRate
                    }).ToList()
            };
            return mobileResponse;
        }

        public async Task<EarnRuleLocalizedModel> GetHistoryAsync(Guid earnRuleId, Localization localization)
        {
            var result = await _campaignRepository.GetByIdAsync(earnRuleId);

            if (result == null)
                return null;

            var mobileResponse = BuildEarnRuleLocalizedModel(localization, result);

            return mobileResponse;
        }

        private async Task PublishCampaignChangeEvent(Guid campaignId, EventCampaignStatus campaignStatus, ActionType actionType)
        {
            var campaignEvent = new CampaignChangeEvent
            {
                Id = Guid.NewGuid(),
                CampaignId = campaignId,
                Status = campaignStatus,
                Action = actionType,
                TimeStamp = DateTime.UtcNow
            };

            _log.Info("Campaign change event published", campaignEvent);

            await _campaignChangeEventPublisher.PublishAsync(campaignEvent);
        }

        private static Money18 CalculateTotalReward(CampaignDetails result)
        {
            var isApproximate = result.RewardType == RewardType.Percentage ||
                                result.RewardType == RewardType.ConversionRate;

            var earnRuleReward = isApproximate && result.ApproximateAward.HasValue ? result.ApproximateAward.Value : result.Reward;

            if (result.Conditions.Count == 1)
            {
                return earnRuleReward;
            }

            Money18 conditionReward = 0m;

            foreach (var condition in result.Conditions)
            {
                var isApproximateCondition = condition.RewardType == RewardType.Percentage ||
                                             condition.RewardType == RewardType.ConversionRate;

                var conditionAmount = isApproximateCondition && condition.ApproximateAward.HasValue ?
                    condition.ApproximateAward.Value : condition.ImmediateReward;

                conditionReward += conditionAmount;
            }

            var totalReward = conditionReward;

            return totalReward;
        }
    }
}
