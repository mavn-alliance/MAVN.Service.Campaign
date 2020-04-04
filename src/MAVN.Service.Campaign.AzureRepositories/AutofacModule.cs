using Autofac;
using AzureStorage.Blob;
using MAVN.Service.Campaign.AzureRepositories.Repositories.File;
using MAVN.Service.Campaign.Domain.Repositories;
using Lykke.SettingsReader;
using System;
using AutoMapper;
using AzureStorage.Tables;
using Lykke.Common.Log;

namespace MAVN.Service.Campaign.AzureRepositories
{
    public class AutofacModule : Module
    {
        private readonly IReloadingManager<string> _rulesImageConnString;

        public AutofacModule(IReloadingManager<string> rulesImageConnString)
        {
            _rulesImageConnString = rulesImageConnString ??
                throw new ArgumentNullException(nameof(rulesImageConnString));
        }

        protected override void Load(ContainerBuilder builder)
        {
            const string rulesTableName = "RulesFiles";

            builder.Register(c =>
                    new FileRepository(AzureBlobStorage.Create(_rulesImageConnString)))
                .As<IFileRepository>()
                .SingleInstance();

            builder.Register(c =>
                    new FileInfoRepository(
                        AzureTableStorage<FileInfoEntity>.Create(
                            _rulesImageConnString, rulesTableName,  c.Resolve<ILogFactory>()),
                        c.Resolve<IMapper>()))
                .As<IFileInfoRepository>()
                .SingleInstance();
        }
    }
}
