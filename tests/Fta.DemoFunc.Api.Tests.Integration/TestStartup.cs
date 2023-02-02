using FluentAssertions.Common;
using Fta.DemoFunc.Api.Interfaces;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Fta.DemoFunc.Api.Tests.Integration
{
    public class TestStartup : Startup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            base.Configure(builder);

            var notificationServiceDescriptor = builder.Services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(INotificationService));

            builder.Services.Remove(notificationServiceDescriptor!);

            builder.Services.AddTransient<INotificationService, MockNotificationService>();

            builder.Services.AddTransient<NotesFunction>();
        }
    }
}
