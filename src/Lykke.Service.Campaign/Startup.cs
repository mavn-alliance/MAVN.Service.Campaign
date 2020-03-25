using System;
using AutoMapper;
using JetBrains.Annotations;
using Lykke.Sdk;
using Lykke.Service.Campaign.Profiles;
using Lykke.Service.Campaign.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Lykke.Service.Campaign
{
    [UsedImplicitly]
    public class Startup
    {
        private readonly LykkeSwaggerOptions _swaggerOptions = new LykkeSwaggerOptions
        {
            ApiTitle = "Campaign API", ApiVersion = "v1"
        };

        [UsedImplicitly]
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            return services.BuildServiceProvider<AppSettings>(options =>
            {
                options.SwaggerOptions = _swaggerOptions;

                options.Extend = (collection, manager) =>
                {
                    collection.Configure<ApiBehaviorOptions>(apiBehaviorOptions =>
                    {
                        apiBehaviorOptions.SuppressModelStateInvalidFilter = true;
                    });

                    collection.AddAutoMapper(
                        typeof(ServiceProfile),
                        typeof(MsSqlRepositories.AutoMapperProfile),
                        typeof(AzureRepositories.AutoMapperProfile));
                };

                options.Logs = logs =>
                {
                    logs.AzureTableName = "CampaignLog";
                    logs.AzureTableConnectionStringResolver = settings => settings.CampaignService.Db.LogsConnString;
                };
            });
        }

        [UsedImplicitly]
        public void Configure(IApplicationBuilder app, IMapper mapper)
        {
            mapper.ConfigurationProvider.AssertConfigurationIsValid();

            app.UseLykkeConfiguration(options => { options.SwaggerOptions = _swaggerOptions; });
        }
    }
}
