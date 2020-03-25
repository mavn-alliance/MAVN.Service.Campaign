using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AzureStorage;
using Lykke.Service.Campaign.Domain.Models;
using Lykke.Service.Campaign.Domain.Repositories;

namespace Lykke.Service.Campaign.AzureRepositories.Repositories.File
{
    public class FileInfoRepository : IFileInfoRepository
    {
        private readonly INoSQLTableStorage<FileInfoEntity> _storage;
        private readonly IMapper _mapper;

        public FileInfoRepository(
            INoSQLTableStorage<FileInfoEntity> storage,
            IMapper mapper)
        {
            _storage = storage;
            _mapper = mapper;
        }

        public async Task<FileModel> GetAsync(string ruleId)
        {
            var entity = await _storage.GetDataAsync(GetPartitionKey(ruleId));

            return _mapper.Map<FileModel>(entity.FirstOrDefault());
        }

        public async Task<string> InsertAsync(FileModel fileInfo)
        {
            var entity = new FileInfoEntity(GetPartitionKey(fileInfo.RuleContentId.ToString()), GetRowKey());

            _mapper.Map(fileInfo, entity);

            await _storage.InsertAsync(entity);

            return entity.RowKey;
        }

        public async Task UpdateAsync(FileModel fileInfo)
        {
            var entity = new FileInfoEntity(GetPartitionKey(fileInfo.RuleContentId.ToString()), fileInfo.Id)
            {
                ETag = "*"
            };

            _mapper.Map(fileInfo, entity);

            await _storage.ReplaceAsync(entity);
        }

        public async Task DeleteAsync(string ruleId)
        {
            var entities = await _storage.GetDataAsync(GetPartitionKey(ruleId));

            await _storage.DeleteAsync(entities.FirstOrDefault());
        }

        private static string GetPartitionKey(string ruleId)
            => ruleId;

        private static string GetRowKey()
            => Guid.NewGuid().ToString("D");
    }
}
