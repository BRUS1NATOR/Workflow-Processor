using MassTransit;
using Microsoft.Extensions.Logging;
using WorkflowProcessor.Core;
using WorkflowProcessor.MasstransitWorkflow.Models;
using WorkflowProcessor.Services;

namespace WorkflowProcessor.MasstransitWorkflow
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
