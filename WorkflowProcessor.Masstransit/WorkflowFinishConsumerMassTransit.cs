using MassTransit;
using Microsoft.Extensions.Logging;
using WorkflowProcessor.Bus.Models;
using WorkflowProcessor.Services;

namespace WorkflowProcessor.Bus
{
    public class WorkflowFinishConsumerMassTransit : WorkflowFinishConsumer, IConsumer<WorkflowInstanceFinishMessage>
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
