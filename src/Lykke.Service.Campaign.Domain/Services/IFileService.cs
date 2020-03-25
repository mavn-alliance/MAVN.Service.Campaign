using System;
using System.Threading.Tasks;
using Lykke.Service.Campaign.Domain.Models;

namespace Lykke.Service.Campaign.Domain.Services
{
    public interface IFileService
    {
        Task<FileResponseModel> GetAsync(Guid ruleContentId);

        Task<string> SaveAsync(FileModel file);

        Task DeleteAsync(Guid ruleContentId);
    }
}
