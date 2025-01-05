using MassTransit;
using MassTransitExample.Services;
using WorkflowProcessor.Services;

namespace MassTransitExample.MasstransitWorkflow
{
    public class WorkflowStartConsumer : IConsumer<WorkflowStartMessage>
    {
        private WorkflowExecutor _workflowManager;
        private WorkflowStorage _workflowStorage;

        public WorkflowStartConsumer(WorkflowExecutor workflowManager, WorkflowStorage workflowStorage)
        {
            _workflowManager = workflowManager;
            _workflowStorage = workflowStorage;
        }
        public async Task Consume(ConsumeContext<WorkflowStartMessage> context)
        {
            var workflow = _workflowStorage.GetWorkflow(context.Message.WorkflowName, context.Message.Version);
            if (workflow is null)
            {
                await Task.FromResult(0);
            }
            await _workflowManager.StartProcessAsync(workflow);
            await Task.FromResult(0);
        }
    }
}
