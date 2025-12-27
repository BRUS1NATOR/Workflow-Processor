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
    public partial class WorkflowTestsFixture : IDisposable
    {
        public WorkflowDbFixture DbFixture { get; private set; }
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
            DbFixture = new WorkflowDbFixture(SqlConnectionString);
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

            var startConsumer = new WorkflowStartConsumer(new FakeLogger<WorkflowStartConsumer>(), WorkflowExecutor, WorkflowStorage);
            var finishConsumer = new WorkflowFinishConsumer(new FakeLogger<WorkflowFinishConsumer>(), null);
            var executeStepConsumer = new WorkflowExecuteStepConsumer(new FakeLogger<WorkflowExecuteStepConsumer>(), WorkflowExecutor, DbFixture.DbContext);
            var messageConsumer = new WorkflowMessageInMemoryConsumer(startConsumer, finishConsumer, executeStepConsumer, channel);
            messageConsumer.StartAsync(CancellationToken.None);
        }

        public void Dispose()
        {
            // clean up code
            DbFixture.Dispose();
        }
    }
}