using MassTransit;
using Microsoft.Extensions.Logging;
using WorkflowProcessor.Bus.Models;
using WorkflowProcessor.Core;
using WorkflowProcessor.Services;

namespace WorkflowProcessor.Bus
{
    public class WorkflowStartConsumerMassTransit : WorkflowStartConsumer, IConsumer<WorkflowStartMessage>
    {
        public WorkflowStartConsumerMassTransit(ILogger<WorkflowStartConsumer> logger, WorkflowExecutor workflowManager, WorkflowStorage workflowStorage) 
            : base(logger, workflowManager, workflowStorage)
        {
        }

        public async Task Consume(ConsumeContext<WorkflowStartMessage> context)
        {
            await ConsumeAsync(context.Message);
        }
    }
}
