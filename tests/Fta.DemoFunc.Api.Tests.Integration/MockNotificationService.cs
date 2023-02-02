using Fta.DemoFunc.Api.Interfaces;
using Fta.DemoFunc.Api.Options;

namespace Fta.DemoFunc.Api.Tests.Integration
{
    public class MockNotificationService : INotificationService
    {
        public Task SendNoteCreatedEventAsync(CreateNoteOptions createNoteOptions, CancellationToken ct)
        {
            return Task.CompletedTask;
        }
    }
}
