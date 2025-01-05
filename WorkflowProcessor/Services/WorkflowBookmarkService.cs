using MassTransitExample.Services;
using WorkflowProcessor.Core.Results;

namespace WorkflowProcessor.Services
{
    public class WorkflowBookmarkService
    {
        private WorkflowInstanceFactory _factory;
        private WorkflowExecutor _workflowExecutor;
        private WorkflowContext _dbContext;

        public WorkflowBookmarkService(WorkflowExecutor workflowExecutor, WorkflowInstanceFactory factory, WorkflowContext dbContext)
        {
            _factory = factory;
            _workflowExecutor = workflowExecutor;
            _dbContext = dbContext;
        }

        public async Task BookmarkCompleteAsync(WorkflowBookmark bookmark, WorkflowBlockingActivityResult? workflowExecutionResult = null)
        {
            if (bookmark.Status != Core.Enums.WorkflowBookmarkStatus.Active)
            {
                return;
            }

            var workflowInstance = _factory.GetWorkflowInstance(bookmark);
            if(workflowInstance is null)
            {
                return;
            }

            if (workflowExecutionResult is null)
            {
                workflowExecutionResult = new WorkflowBlockingActivityResult()
                {
                    WorkflowInstanceId = workflowInstance.Id
                };
            }
            else
            {
                workflowExecutionResult.WorkflowInstanceId = workflowInstance.Id;
            }

            if (await _workflowExecutor.ProcessExecutionResultAsync(workflowInstance, bookmark.WorkflowExecutionPoint, workflowExecutionResult))
            {
                bookmark.Status = Core.Enums.WorkflowBookmarkStatus.Finished;
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
