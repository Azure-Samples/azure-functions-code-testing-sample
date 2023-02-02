using Fta.DemoFunc.Api.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Fta.DemoFunc.Api.Interfaces
{
    public interface INoteRepository
    {
        Task<Note> CreateAsync(Note note, CancellationToken ct);
    }
}
