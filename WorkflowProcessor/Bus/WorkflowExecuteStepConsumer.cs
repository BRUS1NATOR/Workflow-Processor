using Microsoft.EntityFrameworkCore;
using WorkflowProcessor.Core;
using WorkflowProcessor.Bus.Models;
using Microsoft.Extensions.Logging;

namespace WorkflowProcessor.Bus
{
    public class WorkflowExecuteStepConsumer : IWorkflowMessageConsumer<WorkflowExecuteStepMessage>
    {
        protected ILogger<WorkflowExecuteStepConsumer> _logger;
        protected WorkflowDbContext _dbContext;
        protected WorkflowExecutor _workflowExecutor;

        public WorkflowExecuteStepConsumer(ILogger<WorkflowExecuteStepConsumer> logger, WorkflowExecutor workflowExecutor, WorkflowDbContext dbContext)
        {
            _logger = logger;
            _workflowExecutor = workflowExecutor;
            _dbContext = dbContext;
        }

        public WorkflowExecutor WorkflowManager { get; }

        public async Task ConsumeAsync(WorkflowExecuteStepMessage executeStep)
        {
            var workflowInstance = await _dbContext.WorkflowInstances
                .Include(x => x.Context)
                .FirstOrDefaultAsync(x => x.Id == executeStep.WorkflowInstanceId);
            if (workflowInstance is null)
            {
                throw new Exception($"WorkflowInstance with id {executeStep.WorkflowInstanceId} not found");
            }
            await _workflowExecutor.ExecuteStepAsync(workflowInstance, executeStep);
        }
    }
}