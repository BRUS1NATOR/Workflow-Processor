using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using WorkflowProcessor.MasstransitWorkflow;

namespace WorkflowProcessor.MassTransit.Extensions
{
    public static class WorkflowMassTransitInMemoryBusExtension
    {
        public static void AddWorkflowMessageInMemoryBus(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IWorkflowMessageProducer, WorkflowMessageMassTransitProducer>();
        }

        public static void AddInMemoryMassTransit(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddWorkflowMessageInMemoryBus();
            serviceCollection.AddMassTransit(x =>
            {
                x.AddWorkflowMassTransitConsumers();
                x.UsingInMemory((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);
                });
            });
        }
    }
}
