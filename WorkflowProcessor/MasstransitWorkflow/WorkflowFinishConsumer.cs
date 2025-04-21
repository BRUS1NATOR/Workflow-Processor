using MassTransit;
using Microsoft.Extensions.Logging;
using WorkflowProcessor.MasstransitWorkflow.Models;
using WorkflowProcessor.Services;

namespace WorkflowProcessor.MasstransitWorkflow
{
    public class WorkflowFinishConsumer : IConsumer<WorkflowInstanceFinishMessage>
    {
        private ILogger<WorkflowFinishConsumer> _logger;
        private WorkflowBookmarkService _workflowBookmarkService;

        public WorkflowFinishConsumer(ILogger<WorkflowFinishConsumer> logger, WorkflowBookmarkService workflowBookmarkService)
        {
            _logger = logger;
            _workflowBookmarkService = workflowBookmarkService;
        }
        public async Task Consume(ConsumeContext<WorkflowInstanceFinishMessage> context)
        {
            var bookmark = await _workflowBookmarkService.GetBookMarkByWorkflowChild(context.Message.WorkflowInstanceId);
            if (bookmark is null)
            {
                _logger.LogInformation($"No bookmark found for workflow with id {context.Message.WorkflowInstanceId}");
                return;
            }
            await _workflowBookmarkService.BookmarkCompleteAsync(bookmark);
        }
    }
}
