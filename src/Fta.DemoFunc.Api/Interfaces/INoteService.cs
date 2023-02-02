using Fta.DemoFunc.Api.Dtos;
using Fta.DemoFunc.Api.Options;
using System.Threading;
using System.Threading.Tasks;

namespace Fta.DemoFunc.Api.Interfaces
{
    public interface INoteService
    {
        Task<NoteDto> CreateNoteAsync(CreateNoteOptions createNoteOptions, CancellationToken ct);
    }
}
