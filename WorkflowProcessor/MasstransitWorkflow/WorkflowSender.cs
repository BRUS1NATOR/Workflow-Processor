using MassTransit;
using Microsoft.EntityFrameworkCore.Query.Internal;
using WorkflowProcessor.Core;

namespace MassTransitExample
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
        public async Task SendExecuteNext(WorkflowExecuteStep message)
        {
            await _bus.Publish(message);
        }
    }

    public class WorkflowExecuteStep
    {
        public int WorkflowInstanceId { get; set; }
        public string StepId { get; set; }
        public int? PreviousExecutionPointId { get; set; }

        public WorkflowExecuteStep()
        {
            
        }

        public WorkflowExecuteStep(int workflowInstanceId, string nextStepId, int? previousExecutionPointId = null)
        {
            WorkflowInstanceId = workflowInstanceId;
            StepId = nextStepId;
            PreviousExecutionPointId = previousExecutionPointId;
        }
    }

    public class WorkflowStartMessage
    {
        public string WorkflowName { get; set; }
        public int? Version { get; set; }
    }
}
