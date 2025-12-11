using MassTransit;
using Microsoft.Extensions.Logging;
using WorkflowProcessor.MasstransitWorkflow.Models;
using WorkflowProcessor.Services;

namespace WorkflowProcessor.MasstransitWorkflow
{
    public class WorkflowFinishConsumerMassTransit : WorkflowFinishConsumer, IWorkflowFinishConsumer, IConsumer<WorkflowInstanceFinishMessage>
    {
        public WorkflowFinishConsumerMassTransit(ILogger<WorkflowFinishConsumer> logger, WorkflowBookmarkService workflowBookmarkService) 
            : base(logger, workflowBookmarkService)
        {
        }

        public async Task Consume(ConsumeContext<WorkflowInstanceFinishMessage> context)
        {
            await ConsumeAsync(context.Message);
        }
    }
}
