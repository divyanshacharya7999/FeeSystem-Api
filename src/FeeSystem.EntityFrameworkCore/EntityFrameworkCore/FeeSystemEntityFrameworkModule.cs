using Abp.EntityFrameworkCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero.EntityFrameworkCore;
using FeeSystem.EntityFrameworkCore.Seed;
using System;

namespace FeeSystem.EntityFrameworkCore
{
    [DependsOn(
        typeof(FeeSystemCoreModule), 
        typeof(AbpZeroCoreEntityFrameworkCoreModule))]
    public class FeeSystemEntityFrameworkModule : AbpModule
    {
        /* Used it tests to skip dbcontext registration, in order to use in-memory database of EF Core */
        public bool SkipDbContextRegistration { get; set; }

        public bool SkipDbSeed { get; set; }

        public override void PreInitialize()
        {
            if (!SkipDbContextRegistration )
            {
                try
                {
                    Configuration.Modules.AbpEfCore().AddDbContext<FeeSystemDbContext>(options =>
                    {
                        if (options.ExistingConnection != null)
                        {
                            FeeSystemDbContextConfigurer.Configure(options.DbContextOptions, options.ExistingConnection);
                        }
                        else
                        {
                            FeeSystemDbContextConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
                        }
                    });
                }
                catch (Exception ex)
                {
                    Logger.Error("Error during DbContext configuration", ex);
                    throw;
                }
            }
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(FeeSystemEntityFrameworkModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            if (!SkipDbSeed)
            {
                SeedHelper.SeedHostDb(IocManager);
            }
        }
    }
}
