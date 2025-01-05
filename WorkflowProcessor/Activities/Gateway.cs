using WorkflowProcessor.Core;
using WorkflowProcessor.Core.Results;
using WorkflowProcessor.Core.WorkflowElement;
using WorkflowProcessor.Persistance.Context;

namespace WorkflowProcessor.Activities
{
    public interface IGateway : IWorkflowElement
    {

    }

    public class Gateway<T> : WorkflowElement, IGateway
    {
        public Func<IWorkflowInstance, T>? _conditon { get; set; }

        public void SetCondition(Func<IWorkflowInstance, T> condition)
        {
            _conditon = condition;
        }

        public override async Task<WorkflowResult> ExecuteAsync(IWorkflowInstance context)
        {
            if (_conditon is null)
            {
                return await Task.FromResult(WorkflowExecutionResultWithValue.Next(context, null));
            }
            var result = _conditon.Invoke(context);
            return await Task.FromResult(WorkflowExecutionResultWithValue.Next(context, result));
        }
    }

    public class Gateway<TContext, T> : WorkflowElement<TContext>, IGateway where TContext : IContextData, new()
    {
        public Func<TContext, T>? _conditon { get; set; }

        public void SetCondition(Func<TContext, T> condition)
        {
            _conditon = condition;
        }

        public override async Task<WorkflowResult> ExecuteAsync(IWorkflowInstance instance)
        {
            if (_conditon is null)
            {
                return await Task.FromResult(WorkflowExecutionResultWithValue.Next(instance, null));
            }
            var data = Data(instance);
            var result = _conditon.Invoke(data);
            return await Task.FromResult(WorkflowExecutionResultWithValue.Next(instance, result));
        }
    }

}