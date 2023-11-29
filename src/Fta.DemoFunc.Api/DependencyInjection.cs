using Fta.DemoFunc.Api.Interfaces;
using Fta.DemoFunc.Api.Repositories;
using Fta.DemoFunc.Api.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly.Extensions.Http;
using Polly;
using System;
using System.Net.Http;
using Microsoft.Azure.Cosmos.Fluent;

namespace Fta.DemoFunc.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddSingleton(s => {
                return new CosmosClientBuilder(configuration["CosmosDb:ConnectionString"])
                    .Build();
            });

            services.AddTransient<IDateTimeProvider, DateTimeProvider>();
            services.AddScoped<INoteService, NoteService>();
            services.AddScoped<INoteRepository, NoteRepository>();
            services
                .AddHttpClient<INotificationService, NotificationService>(client =>
                {
                    client.BaseAddress = new Uri(configuration.GetValue<string>("NotificationApiUrl"));
                })
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy());

            return services;
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
