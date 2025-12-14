using MassTransit;
using WorkflowProcessor.Core;
using WorkflowProcessor.Bus.Models;
using Microsoft.Extensions.Logging;

namespace WorkflowProcessor.Bus
{
    public class WorkflowExecuteStepConsumerMassTransit : WorkflowExecuteStepConsumer, IConsumer<WorkflowExecuteStepMessage>
    {
        public WorkflowExecuteStepConsumerMassTransit(ILogger<WorkflowExecuteStepConsumer> logger, WorkflowExecutor workflowExecutor, WorkflowDbContext dbContext) : base(logger, workflowExecutor, dbContext)
        {
        }

        public async Task Consume(ConsumeContext<WorkflowExecuteStepMessage> context)
        {
            await ConsumeAsync(context.Message);
        }
    }
}