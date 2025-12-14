using Microsoft.Extensions.Hosting;
using System.Threading.Channels;
using WorkflowProcessor.Bus.Models;

namespace WorkflowProcessor.Bus.InMemory
{
    public class WorkflowMessageInMemoryConsumer : BackgroundService
    {
        private WorkflowStartConsumer _workflowStartConsumer;
        private WorkflowFinishConsumer _workflowFinishConsumer;
        private WorkflowExecuteStepConsumer _workflowExecuteStepConsumer;
        private Channel<IWorkflowMessage> _channel;

        public WorkflowMessageInMemoryConsumer(WorkflowStartConsumer workflowStartConsumer, WorkflowFinishConsumer workflowFinishConsumer,
             WorkflowExecuteStepConsumer workflowExecuteStepConsumer, Channel<IWorkflowMessage> channel)
        {
            _workflowStartConsumer = workflowStartConsumer;
            _workflowFinishConsumer = workflowFinishConsumer;
            _workflowExecuteStepConsumer = workflowExecuteStepConsumer;
            _channel = channel;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (await _channel.Reader.WaitToReadAsync(stoppingToken))
            {
                var message = _channel.Reader.ReadAsync(stoppingToken);
                var data = message.Result;
                switch (data.WorkflowMessageType)
                {
                    case WorkflowMessageType.START:
                        await _workflowStartConsumer.ConsumeAsync((WorkflowStartMessage)data);
                        break;
                    case WorkflowMessageType.FINISH:
                        await _workflowFinishConsumer.ConsumeAsync((WorkflowInstanceFinishMessage)data);
                        break;
                    case WorkflowMessageType.EXECUTE_NEXT_STEP:
                        await _workflowExecuteStepConsumer.ConsumeAsync((WorkflowExecuteStepMessage)data);
                        break;
                }
            }
        }
    }
}
