using AutoMapper;
using Lykke.Service.Campaign.Client.Models.BonusType;
using Lykke.Service.Campaign.Client.Models.BurnRule.Requests;
using Lykke.Service.Campaign.Client.Models.BurnRule.Responses;
using Lykke.Service.Campaign.Client.Models.Campaign.Requests;
using Lykke.Service.Campaign.Client.Models.Campaign.Responses;
using Lykke.Service.Campaign.Client.Models.Condition;
using Lykke.Service.Campaign.Client.Models.Enums;
using Lykke.Service.Campaign.Client.Models.Files.Requests;
using Lykke.Service.Campaign.Client.Models.Files.Responses;
using Lykke.Service.Campaign.Domain.Models;
using Lykke.Service.Campaign.Domain.Models.BurnRules;
using Lykke.Service.Campaign.Domain.Models.EarnRules;
using System;
using Lykke.Service.Campaign.Client.Models;
using EarnRuleContentCreateRequest = Lykke.Service.Campaign.Client.Models.EarnRuleContent.EarnRuleContentCreateRequest;
using EarnRuleContentEditRequest = Lykke.Service.Campaign.Client.Models.EarnRuleContent.EarnRuleContentEditRequest;
using EarnRuleContentResponse = Lykke.Service.Campaign.Client.Models.EarnRuleContent.EarnRuleContentResponse;
using EventCampaignStatus = Lykke.Service.Campaign.Contract.Enums.CampaignStatus;

namespace Lykke.Service.Campaign.Profiles
{
    public class ServiceProfile : Profile
    {
        public ServiceProfile()
        {
            // Campaign
            CreateMap<CampaignDetails, CampaignCreateModel>(MemberList.Source)
                .ForSourceMember(src => src.Id, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.CampaignStatus, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.IsDeleted, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.IsEnabled, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.CreationDate, opt => opt.DoNotValidate())
                .ForMember(src => src.CompletionCount, opt => opt.MapFrom(c =>
                    c.CompletionCount == int.MaxValue ? (int?)null : c.CompletionCount));
            CreateMap<CampaignCreateModel, CampaignDetails>(MemberList.Destination)
                .ForMember(src => src.Id, opt => opt.Ignore())
                .ForMember(src => src.CampaignStatus, opt => opt.Ignore())
                .ForMember(src => src.IsDeleted, opt => opt.Ignore())
                .ForMember(src => src.CreationDate, opt => opt.Ignore())
                .ForMember(src => src.CompletionCount, opt => opt.MapFrom(c =>
                    c.CompletionCount ?? int.MaxValue));

            CreateMap<CampaignDetails, CampaignEditModel>(MemberList.Source)
                .ForSourceMember(src => src.CampaignStatus, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.IsDeleted, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.CreationDate, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.CreatedBy, opt => opt.DoNotValidate())
                .ForMember(src => src.CompletionCount, opt => opt.MapFrom(c =>
                    c.CompletionCount == int.MaxValue ? (int?)null : c.CompletionCount));
            CreateMap<CampaignEditModel, CampaignDetails>(MemberList.Destination)
                .ForMember(src => src.CampaignStatus, opt => opt.Ignore())
                .ForMember(src => src.IsDeleted, opt => opt.Ignore())
                .ForMember(src => src.CreationDate, opt => opt.Ignore())
                .ForMember(src => src.CreatedBy, opt => opt.Ignore())
                .ForMember(src => src.CompletionCount, opt => opt.MapFrom(c =>
                    c.CompletionCount ?? int.MaxValue));

            CreateMap<CampaignDetails, CampaignDetailResponseModel>(MemberList.Source)
                .ForSourceMember(src => src.IsDeleted, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.CreationDate, opt => opt.DoNotValidate())
                .ForMember(src => src.CompletionCount, opt => opt.MapFrom(c =>
                    c.CompletionCount == int.MaxValue ? (int?)null : c.CompletionCount));
            CreateMap<CampaignDetailResponseModel, CampaignDetails>(MemberList.Destination)
                .ForMember(src => src.IsDeleted, opt => opt.Ignore())
                .ForMember(src => src.CreationDate, opt => opt.Ignore())
                .ForMember(src => src.CompletionCount, opt => opt.MapFrom(c =>
                    c.CompletionCount ?? int.MaxValue));
            CreateMap<CampaignInformationResponseModel, CampaignDetails>()
                .ForMember(src => src.Reward, opt => opt.Ignore())
                .ForMember(src => src.AmountInTokens, opt => opt.Ignore())
                .ForMember(src => src.AmountInCurrency, opt => opt.Ignore())
                .ForMember(src => src.UsePartnerCurrencyRate, opt => opt.Ignore())
                .ForMember(src => src.RewardType, opt => opt.Ignore())
                .ForMember(src => src.FromDate, opt => opt.Ignore())
                .ForMember(src => src.ToDate, opt => opt.Ignore())
                .ForMember(src => src.Description, opt => opt.Ignore())
                .ForMember(src => src.IsEnabled, opt => opt.Ignore())
                .ForMember(src => src.Conditions, opt => opt.Ignore())
                .ForMember(src => src.Contents, opt => opt.Ignore())
                .ForMember(src => src.CreationDate, opt => opt.Ignore())
                .ForMember(src => src.CompletionCount, opt => opt.Ignore())
                .ForMember(src => src.CreatedBy, opt => opt.Ignore())
                .ForMember(src => src.ApproximateAward, opt=>opt.Ignore())
                .ForMember(src => src.Order, opt => opt.Ignore());
            CreateMap<CampaignDetails, CampaignInformationResponseModel>();

            CreateMap<CampaignsPaginationRequestModel, CampaignListRequestModel>()
                .ForMember(src => src.CampaignStatus, opt => opt.MapFrom(c => c.CampaignStatus))
                .ForMember(src => src.TotalCount, opt => opt.Ignore());
            CreateMap<Domain.Models.Campaign, CampaignResponse>();
            CreateMap<PaginatedCampaignListModel, PaginatedCampaignListResponseModel>()
                .ForMember(c => c.Campaigns, opt => opt.MapFrom(c => c.Campaigns));
            // Condition
            CreateMap<Condition, ConditionModel>()
                .ForMember(src => src.Type, opt => opt.MapFrom(src => src.BonusType.Type))
                .ForMember(src => src.TypeDisplayName, opt => opt.MapFrom(src => src.BonusType.DisplayName))
                .ForMember(src => src.Vertical, opt => opt.MapFrom(src => src.BonusType.Vertical))
                .ForMember(src => src.IsHiddenType, opt => opt.MapFrom(src => src.BonusType.IsHidden))

                .ForMember(src => src.CompletionCount, opt => opt.MapFrom(c =>
                    c.CompletionCount == int.MaxValue ? (int?)null : c.CompletionCount))
                .ForMember(dest => dest.RewardHasRatio, opt => opt.MapFrom(c => c.BonusType.RewardHasRatio));

            CreateMap<ConditionModel, Condition>()
                .ForMember(dest => dest.BonusType, opt => opt.MapFrom(src => new BonusType
                {
                    Type = src.Type,
                    DisplayName = src.TypeDisplayName,
                    Vertical = src.Vertical,
                    IsHidden = src.IsHiddenType
                }))
                .ForMember(src => src.CompletionCount, opt => opt.MapFrom(c =>
                    c.CompletionCount ?? int.MaxValue))
                .ForSourceMember(src => src.RewardHasRatio, opt => opt.DoNotValidate());

            CreateMap<Condition, ConditionCreateModel>()
                .ForMember(src => src.Type, opt => opt.MapFrom(dest => dest.BonusType.Type))
                .ForSourceMember(src => src.Id, opt => opt.DoNotValidate())
                .ForMember(src => src.CompletionCount, opt => opt.MapFrom(c =>
                    c.CompletionCount == int.MaxValue ? (int?)null : c.CompletionCount))
                .ForMember(dest => dest.RewardHasRatio, opt => opt.MapFrom(c => c.BonusType.RewardHasRatio));

            CreateMap<ConditionCreateModel, Condition>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(src => src.CampaignId, opt => opt.Ignore())
                .ForMember(dest => dest.BonusType, opt => opt.MapFrom(src => new BonusType
                {
                    Type = src.Type
                }))
                .ForMember(src => src.CompletionCount, opt => opt.MapFrom(c =>
                    c.CompletionCount ?? int.MaxValue));
            CreateMap<Condition, ConditionEditModel>(MemberList.Source)
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.BonusType.Type))
                .ForSourceMember(src => src.CampaignId, opt => opt.DoNotValidate())
                .ForMember(src => src.CompletionCount, opt => opt.MapFrom(c =>
                    c.CompletionCount == int.MaxValue ? (int?)null : c.CompletionCount));
            CreateMap<ConditionEditModel, Condition>(MemberList.Destination)
                .ForMember(dest => dest.CampaignId, opt => opt.Ignore())
                .ForMember(dest => dest.BonusType, opt => opt.MapFrom(src => new BonusType
                {
                    Type = src.Type
                }))
                .ForMember(src => src.CompletionCount, opt => opt.MapFrom(c =>
                    c.CompletionCount ?? int.MaxValue))
                .ForSourceMember(src => src.RewardHasRatio, opt => opt.DoNotValidate());

            CreateMap<EarnRuleLocalizedModel, EarnRuleLocalizedResponse>(MemberList.Destination)
                .ForMember(src => src.CompletionCount, opt => opt.MapFrom(c =>
                    c.CompletionCount == int.MaxValue ? (int?)null : c.CompletionCount));

            CreateMap<ConditionLocalizedModel, ConditionLocalizedResponse>(MemberList.Destination)
                .ForMember(src => src.CompletionCount, opt => opt.MapFrom(c =>
                    c.CompletionCount == int.MaxValue ? (int?)null : c.CompletionCount));

            //Condition attributes
            CreateMap<RewardRatioAttribute, RewardRatioAttributeModel>();
            CreateMap<RewardRatioAttributeModel, RewardRatioAttribute>();

            CreateMap<RewardRatioAttributeModel, RewardRatioAttributeDetailsResponseModel>();
            CreateMap<RewardRatioAttributeDetailsResponseModel, RewardRatioAttributeModel>();

            CreateMap<RatioAttributeModel, RatioAttribute>();
            CreateMap<RatioAttribute, RatioAttributeModel>()
                .ForMember(dest => dest.Threshold, opt => opt.Ignore());

            CreateMap<RatioAttributeModel, RatioAttributeDetailsModel>();
            CreateMap<RatioAttributeDetailsModel, RatioAttributeModel>()
                .ForMember(dest => dest.Threshold, opt => opt.Ignore());

            // Bonus Type
            CreateMap<BonusType, BonusTypeModel>(MemberList.Source)
                .ForSourceMember(src => src.IsAvailable, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.CreationDate, opt => opt.DoNotValidate());
            CreateMap<BonusTypeModel, BonusType>(MemberList.Destination)
                .ForMember(dest => dest.IsAvailable, opt => opt.Ignore())
                .ForMember(dest => dest.CreationDate, opt => opt.Ignore());
            CreateMap<BonusType, BonusTypeEditModel>(MemberList.Source)
                .ForSourceMember(src => src.CreationDate, opt => opt.DoNotValidate());
            CreateMap<BonusTypeEditModel, BonusType>(MemberList.Destination)
                .ForMember(dest => dest.CreationDate, opt => opt.Ignore());

            CreateMap<CampaignStatus, EventCampaignStatus>();

            //Earn Rules
            CreateMap<BurnRulePaginationRequest, BurnRuleListRequestModel>()
                .ForMember(src => src.TotalCount, opt => opt.Ignore());

            CreateMap<BurnRuleModel, BurnRuleInfoResponse>(MemberList.Destination);
            CreateMap<BurnRuleInfoResponse, BurnRuleModel>(MemberList.Source)
                .ForMember(dest => dest.PartnerIds, opt => opt.Ignore());

            CreateMap<PaginatedBurnRuleList, PaginatedBurnRuleListResponse>();

            CreateMap<PaginatedEarnRuleListModel, EarnRulePaginatedResponseModel>();
            CreateMap<PaginatedBurnRuleListModel, BurnRulePaginatedResponseModel>();

            CreateMap<BasePaginationRequestModel, PaginationModel>()
                .ForMember(dest => dest.TotalCount, opt => opt.Ignore());

            CreateMap<BurnRuleModel, BurnRuleResponse>()
                .ForSourceMember(src => src.CreatedBy, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.IsDeleted, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.CreationDate, opt => opt.DoNotValidate())
                .ForMember(src => src.BurnRuleContents, opt => opt.MapFrom(e => e.BurnRuleContents))
                .ForMember(src => src.ErrorCode, opt => opt.Ignore())
                .ForMember(src => src.ErrorMessage, opt => opt.Ignore());

            CreateMap<BurnRuleLocalizedModel, BurnRuleLocalizedResponse>(MemberList.Destination);

            CreateMap<BurnRuleContentModel, Client.Models.BurnRuleContent.BurnRuleContentEditRequest>()
                .ForSourceMember(src => src.BurnRuleId, opt => opt.DoNotValidate());

            CreateMap<Client.Models.BurnRuleContent.BurnRuleContentEditRequest, BurnRuleContentModel>()
                .ForMember(s => s.Image, opt => opt.Ignore())
                .ForMember(dest => dest.BurnRuleId, opt => opt.Ignore());

            CreateMap<BurnRuleContentModel, Client.Models.BurnRuleContent.BurnRuleContentCreateRequest>();
            CreateMap<Client.Models.BurnRuleContent.BurnRuleContentCreateRequest, BurnRuleContentModel>()
                .ForMember(e => e.Id, opt => opt.MapFrom(c => Guid.NewGuid()))
                .ForMember(e => e.BurnRuleId, opt => opt.Ignore())
                .ForMember(s => s.Image, opt => opt.Ignore());

            CreateMap<BurnRuleContentModel, Client.Models.BurnRuleContent.BurnRuleContentResponse>();
            CreateMap<Client.Models.BurnRuleContent.BurnRuleContentResponse, BurnRuleContentModel>()
                .ForMember(e => e.Id, opt => opt.Ignore())
                .ForMember(e => e.BurnRuleId, opt => opt.Ignore())
                .ForMember(s => s.Image, opt => opt.MapFrom(s => s.Image));

            CreateMap<BurnRuleCreateRequest, BurnRuleModel>()
               .ForMember(a => a.Id, opt => opt.Ignore())
               .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
               .ForMember(dest => dest.CreationDate, opt => opt.Ignore());

            CreateMap<BurnRuleModel, BurnRuleCreateRequest>()
                .ForSourceMember(a => a.Id, opt => opt.DoNotValidate())
                .ForSourceMember(dest => dest.IsDeleted, opt => opt.DoNotValidate())
                .ForSourceMember(dest => dest.CreationDate, opt => opt.DoNotValidate());

            CreateMap<BurnRuleEditRequest, BurnRuleModel>()
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.CreationDate, opt => opt.Ignore());

            //Spend Rule Content
            CreateMap<EarnRuleContentModel, EarnRuleContentEditRequest>()
                .ForSourceMember(src => src.CampaignId, opt => opt.DoNotValidate());
            CreateMap<EarnRuleContentEditRequest, EarnRuleContentModel>()
                .ForMember(s => s.Image, opt => opt.Ignore())
                .ForMember(dest => dest.CampaignId, opt => opt.Ignore());

            CreateMap<EarnRuleContentModel, EarnRuleContentCreateRequest>();
            CreateMap<EarnRuleContentCreateRequest, EarnRuleContentModel>()
                .ForMember(e => e.Id, opt => opt.MapFrom(c => Guid.NewGuid()))
                .ForMember(e => e.CampaignId, opt => opt.Ignore())
                .ForMember(s => s.Image, opt => opt.Ignore());

            CreateMap<EarnRuleContentModel, EarnRuleContentResponse>();
            CreateMap<EarnRuleContentResponse, EarnRuleContentModel>()
                .ForMember(e => e.Id, opt => opt.Ignore())
                .ForMember(e => e.CampaignId, opt => opt.Ignore());

            //Files
            CreateMap<FileResponse, FileResponseModel>();
            CreateMap<FileResponseModel, FileResponse>();

            CreateMap<FileEditRequest, FileModel>();
            CreateMap<FileModel, FileEditRequest>();

            CreateMap<FileCreateRequest, FileModel>()
                .ForMember(f => f.Id, opt => opt.Ignore());

            CreateMap<FileModel, FileCreateRequest>()
                .ForSourceMember(e => e.Id, opt => opt.DoNotValidate());
        }
    }
}
