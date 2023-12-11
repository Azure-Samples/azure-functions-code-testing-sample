using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Fta.DemoFunc.Api.Interfaces
{
    public interface IDbConnectionFactory
    {
        public Task<IDbConnection> CreateConnectionAsync(CancellationToken ct = default);
    }
}
