using Microsoft.Extensions.DependencyInjection;

namespace WorkflowProcessor.Tests.Fixtures
{
    public class ServiceCollectionFixture
    {
        public ServiceProvider ServiceProvider { get; private set; }
        public ServiceCollectionFixture()
        {
            // Create service collection
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddFakeLogging();
            // Build ServiceProvider
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}