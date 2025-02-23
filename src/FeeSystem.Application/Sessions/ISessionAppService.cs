using System.Threading.Tasks;
using Abp.Application.Services;
using FeeSystem.Sessions.Dto;

namespace FeeSystem.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
