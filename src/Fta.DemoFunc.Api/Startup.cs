using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;

[assembly: FunctionsStartup(typeof(Fta.DemoFunc.Api.Startup))]
namespace Fta.DemoFunc.Api
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configuration = new ConfigurationBuilder()
                 .SetBasePath(Environment.CurrentDirectory)
                 .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                 .AddEnvironmentVariables()
                 .Build();

            builder.Services.AddApplication(configuration);
            builder.Services.AddInfrastructure(configuration);
        }
    }
}
