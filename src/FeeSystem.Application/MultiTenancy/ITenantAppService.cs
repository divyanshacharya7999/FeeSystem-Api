using Abp.Application.Services;
using FeeSystem.MultiTenancy.Dto;

namespace FeeSystem.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

