using Microsoft.Extensions.Logging;
using WorkflowProcessor.Bus.Models;
using WorkflowProcessor.Core;
using WorkflowProcessor.Services;

namespace WorkflowProcessor.Bus
{
    public class WorkflowStartConsumer : IWorkflowMessageConsumer<WorkflowStartMessage>
    {
        protected ILogger<WorkflowStartConsumer> _logger;
        protected WorkflowExecutor _workflowExecutor;
        protected IWorkflowStorage _workflowStorage;

        public WorkflowStartConsumer(ILogger<WorkflowStartConsumer> logger, WorkflowExecutor workflowExecutor, IWorkflowStorage workflowStorage)
        {
            _logger = logger;
            _workflowExecutor = workflowExecutor;
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
            await _workflowExecutor.StartProcessAsync(workflow);
        }
    }
}