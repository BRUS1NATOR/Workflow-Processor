using MassTransit;
using Microsoft.EntityFrameworkCore;
using WorkflowProcessor.Core;
using WorkflowProcessor.MasstransitWorkflow.Models;

namespace WorkflowProcessor.MasstransitWorkflow
{
    public class WorkflowExecuteStepConsumer : IConsumer<WorkflowExecuteStep>
    {
        private WorkflowContext _dbContext;
        private WorkflowExecutor _workflowManager;

        public WorkflowExecuteStepConsumer(WorkflowContext dbContext, WorkflowExecutor workflowManager)
        {
            _dbContext = dbContext;
            _workflowManager = workflowManager;
        }
        public async Task Consume(ConsumeContext<WorkflowExecuteStep> context)
        {
            var workflowInstance = await _dbContext.WorkflowInstances
                .Include(x => x.Context)
                .FirstOrDefaultAsync(x => x.Id == context.Message.WorkflowInstanceId);
            if (workflowInstance == null)
            {
                throw new Exception($"WorkflowInstance with id {context.Message.WorkflowInstanceId} not found");
            }
            await _workflowManager.ExecuteAsync(workflowInstance, context.Message);
        }
    }
}