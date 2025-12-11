using Microsoft.Extensions.DependencyInjection;
using WorkflowProcessor.MasstransitWorkflow.Models;

namespace WorkflowProcessor.MasstransitWorkflow
{
    public class WorkflowMessageInMemoryProducer : IWorkflowMessageProducer
    {
        private readonly IServiceProvider _serviceProvider;

        public WorkflowMessageInMemoryProducer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task SendStart(WorkflowStartMessage message)
        {
            // do not await for inmemory bus
            Task.Run(async () =>
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var service = scope.ServiceProvider.GetRequiredService<WorkflowStartConsumer>();
                    await service.ConsumeAsync(message);
                    return Task.CompletedTask;
                }
            });
            return Task.CompletedTask;
        }

        public Task SendFinish(WorkflowInstanceFinishMessage message)
        {
            Task.Run(async () =>
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var service = scope.ServiceProvider.GetRequiredService<WorkflowFinishConsumer>();
                    await service.ConsumeAsync(message);
                    return Task.CompletedTask;
                }
            });
            return Task.CompletedTask;
        }

        public Task SendExecuteNext(WorkflowExecuteStep message)
        {
            Task.Run(async () =>
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var service = scope.ServiceProvider.GetRequiredService<WorkflowExecuteStepConsumer>();
                    await service.ConsumeAsync(message);
                    return Task.CompletedTask;
                }
            });
            return Task.CompletedTask;
        }
    }
}
