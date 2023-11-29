using Fta.DemoFunc.Api.Interfaces;
using Fta.DemoFunc.Api.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.Http;

namespace Fta.DemoFunc.Api.Tests.Integration
{
    public static class TestHelpers
    {
        public static IHostBuilder ConfigureDefaultTestHost(this IHostBuilder builder, NotesFunctionFixture notesFunctionFixture)
        {
            return
                builder
                    .ConfigureLogging(logging =>
                    {
                        logging.ClearProviders();
                    })
                    .ConfigureServices(services =>
                    {
                        var configuration = new ConfigurationBuilder()
                            .SetBasePath(Environment.CurrentDirectory)
                            .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                            .AddEnvironmentVariables()
                            .Build();

                        services.AddApiServices(configuration);

                        services.RemoveAll(typeof(IHostedService));

                        var cosmosClientDescriptor = services.SingleOrDefault(
                        d => d.ServiceType ==
                            typeof(CosmosClient));

                        services.Remove(cosmosClientDescriptor!);

                        services.AddSingleton(sp =>
                        {
                            var cosmosClientBuilder = new CosmosClientBuilder(notesFunctionFixture.GetCosmosDbConnectionString());

                            cosmosClientBuilder.WithHttpClientFactory(() =>
                            {
                                var httpMessageHandler = new HttpClientHandler
                                {
                                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                                };

                                return new HttpClient(httpMessageHandler);
                            });

                            cosmosClientBuilder.WithConnectionModeGateway();

                            return cosmosClientBuilder.Build();
                        });

                        var notificationServiceDescriptor = services.SingleOrDefault(
                                d => d.ServiceType ==
                                    typeof(INotificationService));

                        services.Remove(notificationServiceDescriptor!);

                        services
                            .AddHttpClient<INotificationService, NotificationService>(client =>
                            {
                                client.BaseAddress = new Uri(notesFunctionFixture.GetNotificationApiServerUrl());
                            });

                        services.AddTransient<NotesFunction>();
                    });
        }
    }
}
