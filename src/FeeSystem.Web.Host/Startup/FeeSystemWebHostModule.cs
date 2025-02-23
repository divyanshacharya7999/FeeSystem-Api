using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using FeeSystem.Configuration;

namespace FeeSystem.Web.Host.Startup
{
    [DependsOn(
       typeof(FeeSystemWebCoreModule))]
    public class FeeSystemWebHostModule: AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public FeeSystemWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(FeeSystemWebHostModule).GetAssembly());
        }
    }
}
