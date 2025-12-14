using WorkflowProcessor.Core;
using WorkflowProcessor.Core.ExecutionResults;
using WorkflowProcessor.Core.WorkflowElement;
using WorkflowProcessor.Persistance.Context;

namespace WorkflowProcessor.Activities.Basic
{
    [ActivityType(BaseAcitivityType.StartActivity)]
    public class StartActivity : WorkflowElement
    {
        private string? _workflowInstanceName { get; set; }

        public StartActivity()
        {
        }


        public void SetWorkflowInstanceName(string workflowInstanceName)
        {
            this._workflowInstanceName = workflowInstanceName;
        }

        public override async Task<ActivityExecutionResult> ExecuteAsync(IWorkflowInstance instance)
        {
            if(_workflowInstanceName is not null)
            {
                instance.Name = this._workflowInstanceName;
            }
            return await Task.FromResult(ActivityExecutionResult.Next(instance));
        }
    }

    [ActivityType(BaseAcitivityType.StartActivity)]
    public class StartActivity<TContextData> : WorkflowElement<TContextData>
        where TContextData : IContextData, new()
    {
        protected Action<Context<TContextData>>? ExecuteCode;
        protected Func<Context<TContextData>, Task>? ExecuteCodeAsync;

        public void Code(Action<Context<TContextData>> ExecuteCode)
        {
            this.ExecuteCode = ExecuteCode;
        }
        public void CodeAsync(Func<Context<TContextData>, Task> ExecuteCodeAsync)
        {
            this.ExecuteCodeAsync = ExecuteCodeAsync;
        }

        public override async Task<ActivityExecutionResult> ExecuteAsync(IWorkflowInstance workflowInstance)
        {
            var data = Data(workflowInstance);
            if (ExecuteCode is not null)
            {
                ExecuteCode.Invoke(data);
            }
            if (ExecuteCodeAsync is not null)
            {
                await ExecuteCodeAsync.Invoke(data);
            }
            return await base.ExecuteAsync(workflowInstance);
        }
    }
}
