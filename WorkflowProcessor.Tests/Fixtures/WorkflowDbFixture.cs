using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace WorkflowProcessor.Tests.Fixtures
{
    public class WorkflowDbFixture : IDisposable
    {
        public WorkflowDbContext DbContext { get; private set; }
        public WorkflowDbFixture(string SqlConnectionString)
        {

            // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
            // at the end of the test (see Dispose below).
            var _connection = new SqliteConnection(SqlConnectionString);
            _connection.Open();

            // These options will be used by the context instances in this test suite, including the connection opened above.
            var _contextOptions = new DbContextOptionsBuilder<WorkflowDbContext>()
                .UseSqlite(_connection)
                .Options;

            DbContext = new WorkflowDbContext(_contextOptions);
            DbContext.Database.EnsureCreated();
        }

        public void Dispose()
        {
            // clean up test data from the database
            DbContext.Dispose();
        }
    }
}