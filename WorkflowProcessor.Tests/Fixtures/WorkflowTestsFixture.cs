using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Testing;
using Moq;
using System.Threading.Channels;
using WorkflowProcessor.Bus;
using WorkflowProcessor.Bus.InMemory;
using WorkflowProcessor.Bus.Models;
using WorkflowProcessor.Core;
using WorkflowProcessor.Services;

namespace WorkflowProcessor.Tests.Fixtures
{
    public class WorkflowTestsFixture : IDisposable
    {
        public DatabaseFixture DbFixture { get; private set; }
        public ServiceCollectionFixture ServiceCollectionFixture { get; private set; }


        public FakeLogger<WorkflowStorage> WorkflowStorageLogger = new();

        public WorkflowStorage WorkflowStorage { get; set; }

        public readonly FakeLogger<WorkflowExecutor> WorkflowExecutorLogger = new();
        public WorkflowExecutor WorkflowExecutor { get; set; }

        public readonly FakeLogger<WorkflowMessageInMemoryProducer> WorkflowMessageInMemoryProducerLogger = new();
        public WorkflowTestsFixture()
        {
            var SqlConnectionString = "Filename=:memory:";//ConfigurationManager.AppSettings["SqlConnectionString"];
            WorkflowStorage = new WorkflowStorage(WorkflowStorageLogger);
            DbFixture = new DatabaseFixture(SqlConnectionString);
            ServiceCollectionFixture = new ServiceCollectionFixture(); 
            //
            var provider = new Mock<IServiceProvider>();

            var channel = Channel.CreateUnbounded<IWorkflowMessage>(new UnboundedChannelOptions
            {
                SingleReader = true,
                SingleWriter = false
            });

            var messageProducer = new WorkflowMessageInMemoryProducer(channel);
            WorkflowExecutor = new WorkflowExecutor(WorkflowExecutorLogger, ServiceCollectionFixture.ServiceProvider,
                DbFixture.DbContext, WorkflowStorage, messageProducer);

            var a = new WorkflowStartConsumer(new FakeLogger<WorkflowStartConsumer>(), WorkflowExecutor, WorkflowStorage);
            var b = new WorkflowFinishConsumer(new FakeLogger<WorkflowFinishConsumer>(), null);
            var c = new WorkflowExecuteStepConsumer(new FakeLogger<WorkflowExecuteStepConsumer>(), WorkflowExecutor, DbFixture.DbContext);
            var messageConsumer = new WorkflowMessageInMemoryConsumer(a, b, c, channel);
            messageConsumer.StartAsync(CancellationToken.None);
        }

        public void Dispose()
        {
            // clean up code
            DbFixture.Dispose();
        }

        public class DatabaseFixture : IDisposable
        {
            public WorkflowDbContext DbContext { get; private set; }
            public DatabaseFixture(string SqlConnectionString)
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
}