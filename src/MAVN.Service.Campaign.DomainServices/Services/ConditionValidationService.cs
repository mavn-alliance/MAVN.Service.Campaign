using MAVN.Service.Campaign.Domain.Models;
using MAVN.Service.Campaign.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MAVN.Service.Campaign.DomainServices.Services
{
    public class ConditionValidationService : IConditionValidationService
    {
        private readonly IBonusTypeValidationService _bonusTypeValidationService;

        private const string CampaignConditionsValidationMessage = "Earn rule's Conditions must not be changed";
        private const string CampaignConditionInvalidIdMessage = "The earn rule does not have any condition with id: {0}";
        private const string ConditionPartnerIdsRepeated = "You can not assign more than once one partner to a condition with id: {0}";

        public ConditionValidationService(IBonusTypeValidationService bonusTypeValidationService)
        {
            _bonusTypeValidationService = bonusTypeValidationService;
        }

        public ValidationResult ValidateConditionsAreNotChanged(IReadOnlyList<Condition> newConditions,
            IReadOnlyList<Condition> oldConditions)
        {
            var validationResult = new ValidationResult();

            if (newConditions.Count != oldConditions.Count)
            {
                validationResult.Add(CampaignConditionsValidationMessage);
                return validationResult;
            }

            foreach (var condition in newConditions)
            {
                var oldCondition = oldConditions.FirstOrDefault(c => condition.Id == c.Id);
                if (oldCondition == null)
                {
                    validationResult.Add(CampaignConditionsValidationMessage);
                    return validationResult;
                }

                validationResult.Add(ValidateConditionPropertiesAreNotChanged(condition, oldCondition));
            }

            return validationResult;
        }

        public ValidationResult ValidateConditionPropertiesAreNotChanged(Condition newCondition, Condition oldCondition)
        {
            var validationResult = new ValidationResult();

            if (newCondition == null)
            {
                throw new ArgumentNullException(nameof(newCondition));
            }

            if (oldCondition == null)
            {
                throw new ArgumentNullException(nameof(oldCondition));
            }

            if (newCondition.CompletionCount != oldCondition.CompletionCount
                || newCondition.ImmediateReward != oldCondition.ImmediateReward
                || newCondition.BonusType.Type != oldCondition.BonusType.Type
                || newCondition.HasStaking != oldCondition.HasStaking
                || newCondition.StakeAmount != oldCondition.StakeAmount
                || newCondition.StakingPeriod != oldCondition.StakingPeriod
                || newCondition.StakeWarningPeriod != oldCondition.StakeWarningPeriod
                || newCondition.StakingRule != oldCondition.StakingRule)
            {
                validationResult.Add(CampaignConditionsValidationMessage);
            }

            if (newCondition.PartnerIds.Any() || oldCondition.PartnerIds.Any())
            {
                if (ArePartnersChanged(newCondition.PartnerIds, oldCondition.PartnerIds))
                {
                    validationResult.Add(CampaignConditionsValidationMessage);
                }
            }

            if (newCondition.RewardRatio != null || oldCondition.RewardRatio != null)
            {
                if (AreRatiosChanged(newCondition.RewardRatio, oldCondition.RewardRatio))
                {
                    validationResult.Add(CampaignConditionsValidationMessage);
                }
            }

            return validationResult;
        }

        private bool ArePartnersChanged(List<Guid> newConditionPartnerIds, List<Guid> oldConditionPartnerIds)
        {
            if (newConditionPartnerIds.Count != oldConditionPartnerIds.Count)
                return true;

            foreach (var id in newConditionPartnerIds)
            {
                if (!oldConditionPartnerIds.Contains(id))
                {
                    return true;
                }
            }

            return false;
        }

        private bool AreRatiosChanged(RewardRatioAttributeModel newRatio, RewardRatioAttributeModel oldRatio)
        {
            var oldRatios = oldRatio != null ? oldRatio.Ratios : new List<RatioAttributeModel>();
            var newRatios = newRatio != null ? newRatio.Ratios : new List<RatioAttributeModel>();

            if (oldRatios.Count != newRatios.Count)
                return true;

            foreach (var ratio in newRatios)
            {
                if (!oldRatios.Any(c => c.AreEqual(ratio)))
                {
                    return true;
                }
            }

            return false;
        }

        public ValidationResult ValidateConditionsHaveValidOrEmptyIds(IReadOnlyList<Condition> newConditions,
            IReadOnlyList<Condition> oldConditions)
        {
            var validationResult = new ValidationResult();

            foreach (var condition in newConditions)
            {
                // We add check for Id here because Id being null is a valid case, it means a new condition should be created
                if (string.IsNullOrEmpty(condition.Id))
                {
                    continue;
                }

                var oldCondition = oldConditions.FirstOrDefault(c => condition.Id == c.Id);
                if (oldCondition == null)
                {
                    validationResult.Add(string.Format(CampaignConditionInvalidIdMessage, condition.Id));
                }
            }

            return validationResult;
        }

        public ValidationResult ValidateConditionsBonusTypes(IReadOnlyList<Condition> conditions)
        {
            var validationResult = new ValidationResult();

            foreach (var condition in conditions)
            {
                validationResult.Add(_bonusTypeValidationService.ValidateBonusType(condition.BonusType.Type, condition.HasStaking));
            }

            return validationResult;
        }

        public ValidationResult ValidateConditionsPartnersIds(IReadOnlyList<Condition> conditions)
        {
            var validationResult = new ValidationResult();

            foreach (var condition in conditions)
            {
                if (condition.PartnerIds != null
                    && condition.PartnerIds.Select(c => c.ToString()).Distinct().Count() != condition.PartnerIds.Count)
                {
                    validationResult.Add(string.Format(ConditionPartnerIdsRepeated, condition.Id));
                }
            }

            return validationResult;
        }
    }
}
