using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SampleApp.Domain.Engine;
using SampleApp.Domain.Repository;
using SampleApp.Domain.Repository.Abstraction;
using SampleApp.Shared;
using SampleApp.Shared.AzureAssets;
using SampleApp.Shared.AzureAssets.Abstraction;
using SampleApp.Shared.Data;
using SampleApp.Shared.Data.Abstraction;
using System;

[assembly: FunctionsStartup(typeof(SampleFunctionApp.SampleFunctionAppStartUp))]
namespace SampleFunctionApp
{
    public class SampleFunctionAppStartUp : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            //Database
            builder.Services.AddDbContext<SampleAppDBContext>(options =>
            options.UseSqlServer(Environment.GetEnvironmentVariable(AppSettingsKey.SampleAppDB_ConnectionString)));
            builder.Services.AddTransient<ISampleAppDBContext, SampleAppDBContext>();

            //Rerposiroty
            builder.Services.AddTransient<IUserDetailsRepository, UserDetailsRepository>();
            builder.Services.AddTransient<IRequestAuditRepository, RequestAuditRepository>();

            //Engine
            builder.Services.AddTransient<IUserDetailsEngine, UserDetailsEngine>();

            //Azure assets
            builder.Services.AddTransient<IServiceBusFactory, ServiceBusFactory>();
            builder.Services.AddTransient<IStorageQueueFactory, StorageQueueFactory>();
        }
    }
}
