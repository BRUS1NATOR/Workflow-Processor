using MassTransit;
using Microsoft.Extensions.Logging;
using WorkflowProcessor.Core;
using WorkflowProcessor.MasstransitWorkflow.Models;
using WorkflowProcessor.Services;

namespace WorkflowProcessor.MasstransitWorkflow
{
    public class WorkflowStartConsumer : IConsumer<WorkflowStartMessage>
    {
        private WorkflowExecutor _workflowManager;
        private WorkflowStorage _workflowStorage;
        private ILogger<WorkflowStartConsumer> _logger;

        public WorkflowStartConsumer(ILogger<WorkflowStartConsumer> logger, WorkflowExecutor workflowManager, WorkflowStorage workflowStorage)
        {
            _logger = logger;
            _workflowManager = workflowManager;
            _workflowStorage = workflowStorage;
        }
        public async Task Consume(ConsumeContext<WorkflowStartMessage> context)
        {
            var workflow = _workflowStorage.GetWorkflow(context.Message.WorkflowName, context.Message.Version);
            if (workflow is null)
            {
                _logger.LogWarning($"WorkflowIsntance with name: {context.Message.WorkflowName} and version: {context.Message.Version} not found");
                return;
            }
            await _workflowManager.StartProcessAsync(workflow);
        }
    }
}
