using Fta.DemoFunc.Api.Options;
using System.Threading;
using System.Threading.Tasks;

namespace Fta.DemoFunc.Api.Interfaces
{
    public interface INotificationService
    {
        Task SendNoteCreatedEventAsync(CreateNoteOptions createNoteOptions, CancellationToken ct);
    }
}
