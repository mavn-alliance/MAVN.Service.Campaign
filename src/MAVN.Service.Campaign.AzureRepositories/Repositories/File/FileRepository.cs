using AzureStorage;
using MAVN.Service.Campaign.Domain.Repositories;
using System;
using System.Threading.Tasks;

namespace MAVN.Service.Campaign.AzureRepositories.Repositories.File
{
    public class FileRepository : IFileRepository
    {
        private readonly IBlobStorage _storage;
        private const string ContainerName = "rulefiles";

        public FileRepository(IBlobStorage storage)
        {
            _storage = storage ??
                throw new ArgumentNullException(nameof(storage));
        }

        public async Task<byte[]> GetAsync(string fileName)
        {
            await _storage.CreateContainerIfNotExistsAsync(ContainerName);

            using (var stream = await _storage.GetAsync(ContainerName, fileName))
            {
                byte[] buffer = new byte[stream.Length];
                await stream.ReadAsync(buffer, 0, (int)stream.Length);
                return buffer;
            }
        }

        public async Task<string> GetBlobUrl(string fileName)
        {
            await _storage.CreateContainerIfNotExistsAsync(ContainerName);

            var hasBlob = await _storage.HasBlobAsync(ContainerName, fileName);

            return hasBlob ?
                _storage.GetBlobUrl(ContainerName, fileName) : null;
        }

        public async Task<string> InsertAsync(byte[] file, string fileName)
        {
            await _storage.CreateContainerIfNotExistsAsync(ContainerName);
            await _storage.SaveBlobAsync(ContainerName, fileName, file);
            return _storage.GetBlobUrl(ContainerName, fileName);
        }

        public async Task DeleteAsync(string id)
        {
            var hasBlob = await _storage.HasBlobAsync(ContainerName, id);

            if (hasBlob)
            {
                await _storage.DelBlobAsync(ContainerName, id);
            }
        }
    }
}
