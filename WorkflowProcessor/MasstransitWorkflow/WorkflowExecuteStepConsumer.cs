using Microsoft.EntityFrameworkCore;
using WorkflowProcessor.Core;
using WorkflowProcessor.MasstransitWorkflow.Models;

namespace WorkflowProcessor.MasstransitWorkflow
{
    public class WorkflowExecuteStepConsumer : IWorkflowExecuteStepConsumer
    {
        protected WorkflowContext _dbContext;
        protected WorkflowExecutor _workflowManager;

        public WorkflowExecuteStepConsumer(WorkflowContext dbContext, WorkflowExecutor workflowManager)
        {
            _dbContext = dbContext;
            _workflowManager = workflowManager;
        }

        public async Task ConsumeAsync(WorkflowExecuteStep executeStep)
        {
            var workflowInstance = await _dbContext.WorkflowInstances
                .Include(x => x.Context)
                .FirstOrDefaultAsync(x => x.Id == executeStep.WorkflowInstanceId);
            if (workflowInstance is null)
            {
                throw new Exception($"WorkflowInstance with id {executeStep.WorkflowInstanceId} not found");
            }
            await _workflowManager.ExecuteAsync(workflowInstance, executeStep);
        }
    }
}