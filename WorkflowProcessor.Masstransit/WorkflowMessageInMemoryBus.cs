using MassTransit;
using WorkflowProcessor.MasstransitWorkflow.Models;

namespace WorkflowProcessor.MasstransitWorkflow
{
    public class WorkflowMessageMassTransitProducer : IWorkflowMessageProducer
    {
        protected IBus _bus;

        public WorkflowMessageMassTransitProducer(IBus bus)
        {
            _bus = bus;
        }

        public async Task SendStart(WorkflowStartMessage message)
        {
            await _bus.Publish(message);
        }

        public async Task SendFinish(WorkflowInstanceFinishMessage message)
        {
            await _bus.Publish(message);
        }

        public async Task SendExecuteNext(WorkflowExecuteStep message)
        {
            await _bus.Publish(message);
        }
    }
}
