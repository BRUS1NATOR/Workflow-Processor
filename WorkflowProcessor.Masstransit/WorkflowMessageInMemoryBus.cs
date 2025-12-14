using MassTransit;
using WorkflowProcessor.Bus.Models;

namespace WorkflowProcessor.Bus
{
    public class WorkflowMessageMassTransitProducer : IWorkflowMessageProducer
    {
        protected IBus _bus;

        public WorkflowMessageMassTransitProducer(IBus bus)
        {
            _bus = bus;
        }

        public async Task SendStartAsync(WorkflowStartMessage message)
        {
            await _bus.Publish(message);
        }

        public async Task SendFinishAsync(WorkflowInstanceFinishMessage message)
        {
            await _bus.Publish(message);
        }

        public async Task SendExecuteNextAsync(WorkflowExecuteStepMessage message)
        {
            await _bus.Publish(message);
        }

        public Task SendMessageAsync(IWorkflowMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
