using System.Threading.Channels;
using WorkflowProcessor.Bus.Models;

namespace WorkflowProcessor.Bus.InMemory
{
    public class WorkflowMessageInMemoryProducer : IWorkflowMessageProducer
    {
        private readonly Channel<IWorkflowMessage> _channel;

        public WorkflowMessageInMemoryProducer(Channel<IWorkflowMessage> channel)
        {
            _channel = channel;
        }

        public async Task SendStartAsync(WorkflowStartMessage message)
        {
            await SendMessageAsync(message);
        }

        public async Task SendFinishAsync(WorkflowInstanceFinishMessage message)
        {
            await SendMessageAsync(message);
        }
        public async Task SendExecuteNextAsync(WorkflowExecuteStepMessage message)
        {
            await SendMessageAsync(message);
        }

        public async Task SendMessageAsync(IWorkflowMessage message)
        {
            await _channel.Writer.WriteAsync(message);
        }
    }
}
