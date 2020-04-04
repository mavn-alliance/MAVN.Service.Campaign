using System;
using System.Threading.Tasks;

namespace MAVN.Service.Campaign.Domain.Repositories
{
    public interface IConfigurationRepository
    {
        Task<DateTime?> Get();

        Task Set(DateTime lastProcessedDate);
    }
}
