using Lykke.Service.Campaign.Domain.Enums;
using Lykke.Service.Campaign.Domain.Models;
using Lykke.Service.Campaign.Domain.Services;
using System.Linq;
using System;

namespace Lykke.Service.Campaign.DomainServices.Services
{
    public class CampaignValidationService : ICampaignValidationService
    {
        private const string CampaignPropertyValidationMessage = "Campaign {0} must not be changed";

        private readonly IConditionValidationService _conditionValidationService;
        private readonly IRuleContentValidationService _contentValidationService;

        public CampaignValidationService(
            IConditionValidationService conditionValidationService, 
            IRuleContentValidationService contentValidationService)
        {
            _conditionValidationService = conditionValidationService;
            _contentValidationService = contentValidationService;
        }

        public ValidationResult ValidateInsert(CampaignDetails campaign)
        {
            var validationResult = new ValidationResult();

            if (campaign.Conditions.Any())
                validationResult.Add(_conditionValidationService.ValidateConditionsBonusTypes(campaign.Conditions));

            return validationResult;
        }

        public ValidationResult ValidateUpdate(CampaignDetails newCampaign, CampaignDetails oldCampaign)
        {
            var validationResult = new ValidationResult();

            if (newCampaign.Conditions.Any())
            {
                validationResult.Add(_conditionValidationService.ValidateConditionsBonusTypes(newCampaign.Conditions));
                validationResult.Add(_conditionValidationService.ValidateConditionsPartnersIds(newCampaign.Conditions));
            }

            if (!validationResult.IsValid)
            {
                return validationResult;
            }

            if(newCampaign.Contents.Any())
                validationResult.Add(_contentValidationService
                    .ValidateHaveInvalidOrEmptyIds(newCampaign.Contents.Select(c=>c.Id).ToList(),
                        oldCampaign.Contents.Select(c => c.Id).ToList()));

            if (!validationResult.IsValid)
            {
                return validationResult;
            }

            // Note: Removed old earn rules validation on edit allowing modification of the earn rule at any time
            //switch (oldCampaign.CampaignStatus)
            //{
            //    case CampaignStatus.Active:
            //        {
            //            validationResult.Add(ValidateUpdateForActiveCampaign(newCampaign, oldCampaign));
            //            break;
            //        }
            //    case CampaignStatus.Inactive:
            //        {
            //            validationResult.Add(ValidateUpdateForInactiveCampaign(newCampaign, oldCampaign));
            //            break;
            //        }
            //    case CampaignStatus.Completed:
            //        {
            //            validationResult.Add(ValidateUpdateForCompletedCampaigns(newCampaign, oldCampaign));
            //            break;
            //        }
            //    case CampaignStatus.Pending:
            //        {
            //            validationResult.Add(ValidateUpdateForPendingCampaigns(newCampaign, oldCampaign));
            //            break;
            //        }
            //    default:
            //        throw new ArgumentOutOfRangeException();
            //}

            return validationResult;
        }

        private ValidationResult ValidateUpdateForActiveCampaign(CampaignDetails campaign, CampaignDetails oldCampaign)
        {
            var validationResult = new ValidationResult();

            if (!campaign.Reward.Equals(oldCampaign.Reward))
            {
                validationResult.Add(string.Format(CampaignPropertyValidationMessage, nameof(campaign.Reward)));
            }

            if (campaign.AmountInTokens != oldCampaign.AmountInTokens)
            {
                validationResult.Add(string.Format(CampaignPropertyValidationMessage, nameof(campaign.AmountInTokens)));
            }

            if (campaign.AmountInCurrency != oldCampaign.AmountInCurrency)
            {
                validationResult.Add(string.Format(CampaignPropertyValidationMessage, nameof(campaign.AmountInCurrency)));
            }

            validationResult.Add(_conditionValidationService.ValidateConditionsAreNotChanged(campaign.Conditions,
                oldCampaign.Conditions));

            return validationResult;
        }

        private ValidationResult ValidateUpdateForInactiveCampaign(CampaignDetails campaign, CampaignDetails oldCampaign)
        {
            var validationResult = new ValidationResult();

            if (!campaign.Name.Equals(oldCampaign.Name, StringComparison.Ordinal))
            {
                validationResult.Add(string.Format(CampaignPropertyValidationMessage, nameof(campaign.Name)));
            }

            if (!campaign.Description.Equals(oldCampaign.Description, StringComparison.Ordinal))
            {
                validationResult.Add(string.Format(CampaignPropertyValidationMessage, nameof(campaign.Description)));
            }

            if (!campaign.Reward.Equals(oldCampaign.Reward))
            {
                validationResult.Add(string.Format(CampaignPropertyValidationMessage, nameof(campaign.Reward)));
            }

            if (campaign.AmountInTokens != oldCampaign.AmountInTokens)
            {
                validationResult.Add(string.Format(CampaignPropertyValidationMessage, nameof(campaign.AmountInTokens)));
            }

            if (campaign.AmountInCurrency != oldCampaign.AmountInCurrency)
            {
                validationResult.Add(string.Format(CampaignPropertyValidationMessage, nameof(campaign.AmountInCurrency)));
            }

            validationResult.Add(_conditionValidationService.ValidateConditionsAreNotChanged(campaign.Conditions,
                oldCampaign.Conditions));

            return validationResult;
        }

        private ValidationResult ValidateUpdateForCompletedCampaigns(CampaignDetails campaign, CampaignDetails oldCampaign)
        {
            var validationResult = new ValidationResult();

            if (!campaign.Name.Equals(oldCampaign.Name, StringComparison.Ordinal))
            {
                validationResult.Add(string.Format(CampaignPropertyValidationMessage, nameof(campaign.Name)));
            }

            if (!campaign.Description.Equals(oldCampaign.Description, StringComparison.Ordinal))
            {
                validationResult.Add(string.Format(CampaignPropertyValidationMessage, nameof(campaign.Description)));
            }

            if (!campaign.FromDate.Equals(oldCampaign.FromDate))
            {
                validationResult.Add(string.Format(CampaignPropertyValidationMessage, nameof(campaign.FromDate)));
            }

            if ((oldCampaign.ToDate == null && campaign.ToDate != null) ||
                (oldCampaign.ToDate != null && campaign.ToDate == null) ||
                (oldCampaign.ToDate != null && campaign.ToDate != null && !campaign.ToDate.Value.Equals(oldCampaign.ToDate.Value)))
            {
                validationResult.Add(string.Format(CampaignPropertyValidationMessage, nameof(campaign.ToDate)));
            }

            if (campaign.Reward != oldCampaign.Reward)
            {
                validationResult.Add(string.Format(CampaignPropertyValidationMessage, nameof(campaign.Reward)));
            }

            if (campaign.AmountInTokens != oldCampaign.AmountInTokens)
            {
                validationResult.Add(string.Format(CampaignPropertyValidationMessage, nameof(campaign.AmountInTokens)));
            }

            if (campaign.AmountInCurrency != oldCampaign.AmountInCurrency)
            {
                validationResult.Add(string.Format(CampaignPropertyValidationMessage, nameof(campaign.AmountInCurrency)));
            }

            if (campaign.IsEnabled != oldCampaign.IsEnabled)
            {
                validationResult.Add(string.Format(CampaignPropertyValidationMessage, nameof(campaign.IsEnabled)));
            }

            validationResult.Add(_conditionValidationService.ValidateConditionsAreNotChanged(campaign.Conditions,
                oldCampaign.Conditions));

            return validationResult;
        }

        private ValidationResult ValidateUpdateForPendingCampaigns(CampaignDetails campaign, CampaignDetails oldCampaign)
        {
            var validationResult = new ValidationResult();

            validationResult.Add(_conditionValidationService.ValidateConditionsHaveValidOrEmptyIds(campaign.Conditions,
                oldCampaign.Conditions));

            return validationResult;
        }
    }
}
