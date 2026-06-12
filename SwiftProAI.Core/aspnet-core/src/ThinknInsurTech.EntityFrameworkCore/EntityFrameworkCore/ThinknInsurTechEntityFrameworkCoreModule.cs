using Abp.Dependency;
using Abp.EntityFrameworkCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Runtime.Security;
using Abp.Zero.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using ThinknInsurTech.Configuration;
using ThinknInsurTech.Migrations.Seed;

namespace ThinknInsurTech.EntityFrameworkCore
{
    [DependsOn(
        typeof(AbpZeroCoreEntityFrameworkCoreModule),
        typeof(ThinknInsurTechCoreModule)
    )]
    public class ThinknInsurTechEntityFrameworkCoreModule : AbpModule
    {
        /* Used it tests to skip DbContext registration, in order to use in-memory database of EF Core */
        public bool SkipDbContextRegistration { get; set; }

        public bool SkipDbSeed { get; set; }

        public override void PreInitialize()
        {
            if (!SkipDbContextRegistration)
            {
                Configuration.Modules.AbpEfCore().AddDbContext<ThinknInsurTechDbContext>(options =>
                {
                    if (options.ExistingConnection != null)
                    {
                        ThinknInsurTechDbContextConfigurer.Configure(options.DbContextOptions,
                            options.ExistingConnection);
                    }
                    else
                    {
                        ThinknInsurTechDbContextConfigurer.Configure(options.DbContextOptions,
                            DecryptConnectionString(options.ConnectionString));
                    }
                });
            }

            // Set this setting to true for enabling entity history.
            Configuration.EntityHistory.IsEnabled = false;

            // Uncomment below line to write change logs for the entities below:
            // Configuration.EntityHistory.Selectors.Add("ThinknInsurTechEntities", EntityHistoryHelper.TrackedTypes);
            // Configuration.CustomConfigProviders.Add(new EntityHistoryConfigProvider(Configuration));

            // Register and enable OU Filter to all database queries
            Configuration.UnitOfWork.RegisterFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit, true);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ThinknInsurTechEntityFrameworkCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            var configurationAccessor = IocManager.Resolve<IAppConfigurationAccessor>();

            using (var scope = IocManager.CreateScope())
            {
                if (!SkipDbSeed && scope.Resolve<DatabaseCheckHelper>()
                        .Exist(configurationAccessor.Configuration["ConnectionStrings:Default"]))
                {
                    SeedHelper.SeedHostDb(IocManager);
                }
            }
        }

        private string DecryptConnectionString(string connectionString)
        {
            var configurationAccessor = IocManager.Resolve<IAppConfigurationAccessor>();

            var UseEncryptedConnectionString = bool.Parse(configurationAccessor.Configuration["UseEncryptedConnectionString"]);
            return UseEncryptedConnectionString ? SimpleStringCipher.Instance.Decrypt(connectionString, "9c6103b656914cb48e0eb40fe779e4e6") : connectionString; // Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        }
    }
}