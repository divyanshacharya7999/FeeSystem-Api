using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using FeeSystem.Authorization;

namespace FeeSystem
{
    [DependsOn(
        typeof(FeeSystemCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class FeeSystemApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<FeeSystemAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(FeeSystemApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)

            );
        }
    }
}
