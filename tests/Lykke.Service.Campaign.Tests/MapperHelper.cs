using AutoMapper;
using Lykke.Service.Campaign.MsSqlRepositories;

namespace Lykke.Service.Campaign.Tests
{
    public static class MapperHelper
    {
        public static IMapper CreateAutoMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddMaps(typeof(AutoMapperProfile)));

            return config.CreateMapper();
        }
    }
}
