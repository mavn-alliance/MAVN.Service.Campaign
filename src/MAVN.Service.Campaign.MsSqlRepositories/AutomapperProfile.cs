using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Common;
using MAVN.Service.Campaign.Domain.Enums;
using MAVN.Service.Campaign.Domain.Models;
using MAVN.Service.Campaign.Domain.Models.BurnRules;
using MAVN.Service.Campaign.Domain.Models.EarnRules;
using MAVN.Service.Campaign.MsSqlRepositories.Entities;
using Newtonsoft.Json;

namespace MAVN.Service.Campaign.MsSqlRepositories
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CampaignDetails, CampaignEntity>(MemberList.Source)
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Parse(src.Id)))
                .ForMember(dest => dest.ConditionEntities, opt => opt.MapFrom(src => src.Conditions))
                .ForSourceMember(src => src.CampaignStatus, opt => opt.DoNotValidate())
                .ForMember(src => src.EarnRuleContents, opt => opt.MapFrom(src => src.Contents));

            CreateMap<CampaignEntity, CampaignDetails>(MemberList.Destination)
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString("D")))
                .ForMember(dest => dest.Conditions, opt => opt.MapFrom(src => src.ConditionEntities))
                .ForMember(dest => dest.CampaignStatus, opt => opt.Ignore())
                .ForMember(dest => dest.Contents, opt => opt.MapFrom(src => src.EarnRuleContents))
                .ForMember(dest => dest.RewardType, opt => opt.MapFrom(src => src.RewardType ?? RewardType.Fixed));

            CreateMap<Domain.Models.Campaign, CampaignEntity>(MemberList.Source)
                .ForMember(dest => dest.ConditionEntities, opt => opt.MapFrom(src => src.Conditions))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Parse(src.Id)))
                .ForSourceMember(src => src.CampaignStatus, opt => opt.DoNotValidate());

            CreateMap<CampaignEntity, Domain.Models.Campaign>(MemberList.Destination)
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString("D")))
                .ForMember(dest => dest.CampaignStatus, opt => opt.Ignore())
                .ForMember(dest => dest.Conditions, opt => opt.MapFrom(src => src.ConditionEntities))
                .ForMember(dest => dest.RewardType, opt => opt.MapFrom(src => src.RewardType ?? RewardType.Fixed));

            CreateMap<Condition, ConditionEntity>(MemberList.Source)
                .ForMember(dest => dest.ConditionPartners,
                    opt => opt.MapFrom(src => src.PartnerIds.Select(c =>
                        new ConditionPartnerEntity { PartnerId = c })))
                .ForSourceMember(src => src.PartnerIds, opt => opt.DoNotValidate())
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Parse(src.Id)))
                .ForMember(dest => dest.CampaignEntityId, opt => opt.MapFrom(src => src.CampaignId))
                .ForMember(dest => dest.BonusTypeName, opt => opt.MapFrom(src => src.BonusType.Type))
                .ForMember(dest => dest.Attributes,
                       opt =>
                      {
                          opt.PreCondition(src => src.RewardRatio != null);
                          opt.MapFrom(s => new List<RewardRatioAttributeModel>() { s.RewardRatio });
                      });

            CreateMap<ConditionEntity, Condition>(MemberList.Destination)
                .ForMember(dest => dest.PartnerIds,
                    opt => opt.MapFrom(src => src.ConditionPartners.Select(p => p.PartnerId).ToArray()))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString("D")))
                .ForMember(dest => dest.CampaignId, opt => opt.MapFrom(src => src.CampaignEntityId.ToString("D")))
                .ForMember(dest => dest.BonusType, opt => opt.MapFrom(src => src.BonusTypeEntity))
                .ForMember(dest => dest.RewardRatio,
                    opt => opt.MapFrom(src => src.Attributes.FirstOrDefault(attr => attr.Type == ConditionAttributeType.Ratio)));

            CreateMap<BonusType, BonusTypeEntity>(MemberList.Source);

            CreateMap<BonusTypeEntity, BonusType>(MemberList.Destination);

            CreateMap<BurnRuleModel, BurnRuleEntity>(MemberList.Source)
                .ForMember(dest => dest.BurnRulePartners,
                    opt => opt.MapFrom(src => src.PartnerIds.Select(c =>
                        new BurnRulePartnerEntity() { PartnerId = c })))
                .ForSourceMember(src => src.PartnerIds, opt => opt.DoNotValidate());

            CreateMap<BurnRuleEntity, BurnRuleModel>(MemberList.Destination)
                .ForMember(dest => dest.PartnerIds,
                    opt => opt.MapFrom(src => src.BurnRulePartners.Select(p => p.PartnerId).ToArray()));

            CreateMap<BurnRuleContentEntity, BurnRuleContentModel>(MemberList.Destination)
                .ForMember(e => e.Image, opt => opt.Ignore());
            CreateMap<BurnRuleContentModel, BurnRuleContentEntity>(MemberList.Source)
                .ForSourceMember(src => src.Image, opt => opt.DoNotValidate());

            CreateMap<EarnRuleContentEntity, EarnRuleContentModel>(MemberList.Destination)
                .ForMember(e => e.Image, opt => opt.Ignore());

            CreateMap<EarnRuleContentModel, EarnRuleContentEntity>(MemberList.Source)
                .ForSourceMember(src => src.Image, opt => opt.DoNotValidate());

            CreateMap<CampaignEntity, CampaignDto>(MemberList.Destination);

            //Condition attribute
            CreateMap<RewardRatioAttributeModel, ConditionAttributeEntity>()
                .ForMember(dest => dest.Type,
                    opt => opt.MapFrom(src => ConditionAttributeType.Ratio))
                .ForMember(src => src.JsonValue,
                    opt => opt.MapFrom(src => JsonConvert.SerializeObject(src.Ratios)))
                .ForMember(c => c.Value, opt => opt.MapFrom(c => c.Ratios))
                .ForMember(c => c.Id, opt => opt.Ignore())
                .ForMember(c => c.Condition, opt => opt.Ignore())
                .ForMember(c => c.ConditionId, opt => opt.Ignore());

            CreateMap<ConditionAttributeEntity, RewardRatioAttributeModel>()
                .ForMember(dest => dest.Ratios,
                    opt => opt.MapFrom(src =>
                        JsonConvert.DeserializeObject<IReadOnlyList<RatioAttributeModel>>(src.JsonValue)));
        }
    }
}
