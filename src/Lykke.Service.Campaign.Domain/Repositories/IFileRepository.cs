using System.Threading.Tasks;

namespace Lykke.Service.Campaign.Domain.Repositories
{
    public interface IFileRepository
    {
        Task<byte[]> GetAsync(string fileName);

        Task<string> GetBlobUrl(string fileName);

        Task<string> InsertAsync(byte[] file, string fileName);

        Task DeleteAsync(string fileName);
    }
}
