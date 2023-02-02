using Fta.DemoFunc.Api.Interfaces;
using Fta.DemoFunc.Api.Logging;
using Fta.DemoFunc.Api.Repositories;
using Fta.DemoFunc.Api.Services;
using Fta.DemoFunc.Api.Settings;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;

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

            var cosmosDbSettings = new CosmosDbSettings();
            configuration.Bind(CosmosDbSettings.CosmosDbSectionKey, cosmosDbSettings);
            builder.Services.AddSingleton(cosmosDbSettings);

            builder.Services.AddTransient(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));
            builder.Services.AddTransient<IDateTimeProvider, DateTimeProvider>();
            builder.Services.AddScoped<INoteService, NoteService>();
            builder.Services.AddScoped<INoteRepository, NoteRepository>();
            builder.Services
                .AddHttpClient<INotificationService, NotificationService>(client =>
                {
                    client.BaseAddress = new Uri(configuration.GetValue<string>("NotificationApiUrl"));
                })
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy());
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrTransientHttpStatusCode()
                .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrTransientHttpStatusCode()
                .CircuitBreakerAsync(3, TimeSpan.FromMinutes(2));
        }
    }
}
