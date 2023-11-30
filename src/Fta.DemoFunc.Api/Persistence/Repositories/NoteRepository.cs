using Dapper;
using Fta.DemoFunc.Api.Entities;
using Fta.DemoFunc.Api.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Fta.DemoFunc.Api.Persistence.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly DatabaseInitializer _databaseInitializer;

        public NoteRepository(IDbConnectionFactory connectionFactory, DatabaseInitializer databaseInitializer)
        {
            _connectionFactory = connectionFactory;
            _databaseInitializer = databaseInitializer;
        }

        public async Task<Note> CreateAsync(Note note, CancellationToken ct = default)
        {
            await _databaseInitializer.InitializeAsync();

            using var connection = await _connectionFactory.CreateConnectionAsync(ct);

            var result = await connection.ExecuteAsync(
                @"
                    INSERT INTO Notes (Id, Title, Body, CreatedAt, LastUpdatedOn) 
                    VALUES (@Id, @Title, @Body, @CreatedAt, @LastUpdatedOn)
                ",
                note
            );

            return note;
        }
    }
}
