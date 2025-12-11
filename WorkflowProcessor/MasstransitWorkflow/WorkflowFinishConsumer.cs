using Microsoft.Extensions.Logging;
using WorkflowProcessor.MasstransitWorkflow.Models;
using WorkflowProcessor.Services;

namespace WorkflowProcessor.MasstransitWorkflow
{
    public class WorkflowFinishConsumer : IWorkflowFinishConsumer
    {
        protected ILogger<WorkflowFinishConsumer> _logger;
        protected WorkflowBookmarkService _workflowBookmarkService;

        public WorkflowFinishConsumer(ILogger<WorkflowFinishConsumer> logger, WorkflowBookmarkService workflowBookmarkService)
        {
            _logger = logger;
            _workflowBookmarkService = workflowBookmarkService;
        }

        public async Task ConsumeAsync(WorkflowInstanceFinishMessage message)
        {
            var finishedWfInstanceId = message.WorkflowInstanceId;
            var bookmark = await _workflowBookmarkService.GetBookMarkByWorkflowChild(message.WorkflowInstanceId);
            if (bookmark is null)
            {
                _logger.LogWarning($"No bookmark found for workflow with id {message.WorkflowInstanceId}");
                return;
            }
            await _workflowBookmarkService.BookmarkCompleteAsync(bookmark);
        }
    }
}