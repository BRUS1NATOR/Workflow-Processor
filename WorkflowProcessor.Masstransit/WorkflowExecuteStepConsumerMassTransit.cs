using MassTransit;
using WorkflowProcessor.Core;
using WorkflowProcessor.MasstransitWorkflow.Models;

namespace WorkflowProcessor.MasstransitWorkflow
{
    public class WorkflowExecuteStepConsumerMassTransit : WorkflowExecuteStepConsumer, IConsumer<WorkflowExecuteStep>, IWorkflowExecuteStepConsumer
    {
        public WorkflowExecuteStepConsumerMassTransit(WorkflowContext dbContext, WorkflowExecutor workflowManager) : base(dbContext, workflowManager)
        {
            _dbContext = dbContext;
            _workflowManager = workflowManager;
        }

        public async Task Consume(ConsumeContext<WorkflowExecuteStep> context)
        {
            await ConsumeAsync(context.Message);
        }
    }
}