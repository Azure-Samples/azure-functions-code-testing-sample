using Fta.DemoFunc.Api.Interfaces;
using Fta.DemoFunc.Api.Persistence;
using Fta.DemoFunc.Api.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

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

                        services.AddApplication(configuration);
                        services.AddInfrastructure(configuration);

                        services.RemoveAll(typeof(IHostedService));

                        services.RemoveAll(typeof(IDbConnectionFactory));
                        services.AddSingleton<IDbConnectionFactory>(_ =>
                            new NpgsqlConnectionFactory(notesFunctionFixture.GetPostgresDbConnectionString()));

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
