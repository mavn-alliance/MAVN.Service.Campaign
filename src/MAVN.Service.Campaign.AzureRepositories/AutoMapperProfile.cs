using AutoMapper;
using MAVN.Service.Campaign.AzureRepositories.Repositories.File;
using MAVN.Service.Campaign.Domain.Models;

namespace MAVN.Service.Campaign.AzureRepositories
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<FileModel, FileInfoEntity>(MemberList.Source)
                .ForSourceMember(c=>c.Content, opt=>opt.DoNotValidate())
                .ForSourceMember(c => c.Id, opt => opt.DoNotValidate());

            CreateMap<FileInfoEntity, FileModel>(MemberList.Destination)
                .ForMember(dest => dest.Content, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.RowKey));
        }
    }
}
