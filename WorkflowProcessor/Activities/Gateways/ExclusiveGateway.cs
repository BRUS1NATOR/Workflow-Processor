using WorkflowProcessor.Core;
using WorkflowProcessor.Core.ExecutionResults;
using WorkflowProcessor.Core.Step;
using WorkflowProcessor.Core.WorkflowElement;
using WorkflowProcessor.Persistance.Context;

namespace WorkflowProcessor.Activities.Gateways
{
    [ActivityType(BaseAcitivityType.Gateway)]
    public class ExclusiveGateway<T> : WorkflowElement, IGateway
    {
        public Func<IWorkflowInstance, T>? _conditon { get; set; }

        public void SetCondition(Func<IWorkflowInstance, T> condition)
        {
            _conditon = condition;
        }

        public override async Task<ActivityExecutionResult> ExecuteAsync(IWorkflowInstance instance)
        {
            if (_conditon is null)
            {
                return await Task.FromResult(ActivityExecutionResultWithValue.Next(instance, null));
            }
            var result = _conditon.Invoke(instance);
            return await Task.FromResult(ActivityExecutionResultWithValue.Next(instance, result));
        }
    }

    [ActivityType(BaseAcitivityType.Gateway)]
    public class ExclusiveGateway<TContextData, T> : WorkflowElement<TContextData>, IGateway
        where TContextData : IContextData, new()
    {
        public Func<Context<TContextData>, T>? _conditon { get; set; }

        public void SetCondition(Func<Context<TContextData>, T> condition)
        {
            _conditon = condition;
        }

        public override async Task<ActivityExecutionResult> ExecuteAsync(IWorkflowInstance instance)
        {
            if (_conditon is null)
            {
                return await Task.FromResult(ActivityExecutionResultWithValue.Next(instance, null));
            }
            var data = Data(instance);
            var result = _conditon.Invoke(data);
            return await Task.FromResult(ActivityExecutionResultWithValue.Next(instance, result));
        }
    }

}