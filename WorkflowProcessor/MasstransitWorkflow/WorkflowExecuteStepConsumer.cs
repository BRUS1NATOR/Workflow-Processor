using MassTransit;
using MassTransitExample.Services;
using WorkflowProcessor.Services;

namespace MassTransitExample.MasstransitWorkflow
{
    public class WorkflowExecuteStepConsumer : IConsumer<WorkflowExecuteStep>
    {
        private WorkflowExecutor _workflowManager;
        private WorkflowInstanceFactory _factory;

        public WorkflowExecuteStepConsumer(WorkflowExecutor workflowManager, WorkflowInstanceFactory factory)
        {
            _workflowManager = workflowManager;
            _factory = factory;
        }
        public async Task Consume(ConsumeContext<WorkflowExecuteStep> context)
        {
            var workflowInstance = await _factory.GetWorkflowInstance(context.Message.WorkflowInstanceId);

            await _workflowManager.ExecuteAsync(workflowInstance, context.Message);
        }
    }
}