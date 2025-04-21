using MassTransit;
using WorkflowProcessor.MasstransitWorkflow.Models;

namespace WorkflowProcessor.MasstransitWorkflow
{
    public class WorkflowSender
    {
        ISendEndpointProvider _sendEndpointProvider { get; set; }
        IBus _bus { get; set; }
        public WorkflowSender(ISendEndpointProvider sendEndpointProvider, IBus bus)
        {
            _sendEndpointProvider = sendEndpointProvider;
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
