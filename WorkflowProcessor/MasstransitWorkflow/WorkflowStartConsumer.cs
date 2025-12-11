using Microsoft.Extensions.Logging;
using WorkflowProcessor.Core;
using WorkflowProcessor.MasstransitWorkflow.Models;
using WorkflowProcessor.Services;

namespace WorkflowProcessor.MasstransitWorkflow
{
    public class WorkflowStartConsumer : IWorkflowStartConsumer
    {
        protected WorkflowExecutor _workflowManager;
        protected WorkflowStorage _workflowStorage;
        protected ILogger<WorkflowStartConsumer> _logger;

        public WorkflowStartConsumer(ILogger<WorkflowStartConsumer> logger, WorkflowExecutor workflowManager, WorkflowStorage workflowStorage)
        {
            _logger = logger;
            _workflowManager = workflowManager;
            _workflowStorage = workflowStorage;
        }
        public async Task ConsumeAsync(WorkflowStartMessage message)
        {
            var workflow = _workflowStorage.GetWorkflow(message.WorkflowName, message.Version);
            if (workflow is null)
            {
                _logger.LogWarning($"WorkflowIsntance with name: {message.WorkflowName} and version: {message.Version} not found");
                return;
            }
            await _workflowManager.StartProcessAsync(workflow);
        }
    }
}