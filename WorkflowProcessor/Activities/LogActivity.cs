using Microsoft.Extensions.Logging;
using WorkflowProcessor.Core;
using WorkflowProcessor.Core.ExecutionResults;
using WorkflowProcessor.Core.Step;
using WorkflowProcessor.Core.WorkflowElement;
using WorkflowProcessor.Persistance.Context;

namespace WorkflowProcessor.Activities
{
    [ActivityType(BaseAcitivityType.LogActivity)]
    public class LogActivity : WorkflowElement
    {
        private ILogger<LogActivity> _logger;
        private string _message { get; set; } = "";

        public LogActivity(ILogger<LogActivity> logger)
        {
            _logger = logger;
        }

        public void Log(string message)
        {
            _message = message;
        }

        public override async Task<ActivityExecutionResult> ExecuteAsync(IWorkflowInstance instance)
        {
            _logger.LogInformation(_message);
            return await Task.FromResult(ActivityExecutionResult.Next(instance));
        }
    }

    [ActivityType(BaseAcitivityType.LogActivity)]
    public class LogActivity<TContextData> : WorkflowElement<TContextData>
        where TContextData : IContextData, new()
    {
        private ILogger<LogActivity> _logger;
        private string _message { get; set; } = "message not defined";
        public Func<Context<TContextData>, string>? MessageFromContext;

        public LogActivity(ILogger<LogActivity> logger)
        {
            _logger = logger;
        }

        public void Log(string message)
        {
            _message = message;
        }

        public void Log(Func<Context<TContextData>, string> message)
        {
            MessageFromContext = message;
        }

        public override async Task<ActivityExecutionResult> ExecuteAsync(IWorkflowInstance instance)
        {
            if (MessageFromContext is not null)
            {
                _logger.LogInformation(MessageFromContext.Invoke(Data(instance)));
                return await Task.FromResult(ActivityExecutionResult.Next(instance));
            }

            _logger.LogInformation(_message);
            return await Task.FromResult(ActivityExecutionResult.Next(instance));
        }
    }
}