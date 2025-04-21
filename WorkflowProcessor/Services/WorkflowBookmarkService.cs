using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WorkflowProcessor.Core;
using WorkflowProcessor.Core.ExecutionResults;

namespace WorkflowProcessor.Services
{
    public class WorkflowBookmarkService
    {
        private ILogger<WorkflowBookmarkService> _logger;
        private WorkflowExecutor _workflowExecutor;
        private WorkflowContext _dbContext;

        public WorkflowBookmarkService(ILogger<WorkflowBookmarkService> logger, WorkflowExecutor workflowExecutor, WorkflowContext dbContext)
        {
            _logger = logger;
            _workflowExecutor = workflowExecutor;
            _dbContext = dbContext;
        }

        public async Task<WorkflowBookmark?> GetBookMarkByWorkflowChild(long workflowChildId)
        {
            return await _dbContext.WorkflowBookmarks
                .Include(x => x.WorkflowExecutionPoint)
                .ThenInclude(x => x.WorkflowInstance)
                .Where(x => x.WorkflowChildId == workflowChildId).FirstOrDefaultAsync();
        }

        public async Task<WorkflowExecutionResult> BookmarkCompleteAsync(string bookmarkName, ActivityExecutionResultWithValue? workflowExecutionResult = null)
        {
            var bookmark = _dbContext.WorkflowBookmarks
               .Include(x => x.WorkflowExecutionPoint)
               .ThenInclude(x => x.WorkflowInstance)
               .FirstOrDefault(x => x.Name == bookmarkName && x.Status == Core.Enums.WorkflowBookmarkStatus.Active);

            if (bookmark is null)
            {
                _logger.LogWarning($"Active bookmark with name \"{bookmarkName}\" not found");
                return new WorkflowExecutionResult(false, $"Active bookmark with name \"{bookmarkName}\" not found");
            }

            return await BookmarkCompleteAsync(bookmark, workflowExecutionResult);
        }

        public async Task<WorkflowExecutionResult> BookmarkCompleteAsync(WorkflowBookmark bookmark, ActivityExecutionResultWithValue? workflowExecutionResult = null)
        {
            if (bookmark.Status != Core.Enums.WorkflowBookmarkStatus.Active)
            {
                return new WorkflowExecutionResult(false, $"Bookmark is not active");
            }

            if (workflowExecutionResult is null)
            {
                workflowExecutionResult = new ActivityExecutionResultWithValue()
                {
                    WorkflowInstanceId = bookmark.WorkflowExecutionPoint.WorkflowInstanceId
                };
            }
            else
            {
                workflowExecutionResult.WorkflowInstanceId = bookmark.WorkflowExecutionPoint.WorkflowInstanceId;
            }

            var executionResult = await _workflowExecutor.ProcessExecutionResultAsync(bookmark.WorkflowExecutionPoint.WorkflowInstance, bookmark.WorkflowExecutionPoint, workflowExecutionResult);
            if (executionResult.IsSuccess)
            {
                bookmark.Status = Core.Enums.WorkflowBookmarkStatus.Finished;
                await _dbContext.SaveChangesAsync();
            }

            return executionResult;
        }
    }
}
