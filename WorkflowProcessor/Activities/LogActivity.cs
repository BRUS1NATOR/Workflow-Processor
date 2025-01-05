using Microsoft.Extensions.Logging;
using WorkflowProcessor.Core;
using WorkflowProcessor.Core.Results;
using WorkflowProcessor.Core.WorkflowElement;
using WorkflowProcessor.Persistance.Context;

namespace WorkflowProcessor.Activities
{
    public class LogActivity : WorkflowElement
    {
        private ILogger<LogActivity> _logger;
        private string _message { get; set; }


        public LogActivity(ILogger<LogActivity> logger)
        {
            _logger = logger;
        }

        public void Log(string message)
        {
            _message = message;
        }

        public override async Task<WorkflowResult> ExecuteAsync(IWorkflowInstance instance)
        {
            _logger.LogInformation(_message);
            return await Task.FromResult(WorkflowResult.Next(instance));
        }
    }


    public class LogActivity<TContext> : WorkflowElement<TContext> where TContext : IContextData, new()
    {
        private ILogger<LogActivity> _logger;
        private string _message { get; set; }
        public Func<TContext, string> MessageFromContext;

        public LogActivity(ILogger<LogActivity> logger)
        {
            _logger = logger;
        }

        public void Log(string message)
        {
            _message = message;
        }

        public void Log(Func<TContext, string> message)
        {
            MessageFromContext = message;
        }

        public override async Task<WorkflowResult> ExecuteAsync(IWorkflowInstance instance)
        {
            if (MessageFromContext is not null)
            {
                _logger.LogInformation(MessageFromContext.Invoke(Data(instance)));
                return await Task.FromResult(WorkflowResult.Next(instance));
            }

            _logger.LogInformation(_message);
            return await Task.FromResult(WorkflowResult.Next(instance));
        }
    }
}