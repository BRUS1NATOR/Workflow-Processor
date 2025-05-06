using WorkflowProcessor.Core;
using WorkflowProcessor.Core.ExecutionResults;
using WorkflowProcessor.Core.Step;
using WorkflowProcessor.Core.WorkflowElement;
using WorkflowProcessor.Persistance.Context;

namespace WorkflowProcessor.Activities.Basic
{
    [ActivityType(BaseAcitivityType.SleepActivity)]
    public class SleepActivity<TContextData> : WorkflowElement<TContextData>
        where TContextData : IContextData, new()
    {
        private int _time { get; set; }
        public Func<Context<TContextData>, int>? TimeToSleepFromContext;

        public void Sleep(int time)
        {
            _time = time;
        }

        public void Sleep(Func<Context<TContextData>, int> timeFunc)
        {
            TimeToSleepFromContext = timeFunc;
        }

        public override async Task<ActivityExecutionResult> ExecuteAsync(IWorkflowInstance instance)
        {
            if (TimeToSleepFromContext is not null)
            {
                await Task.Delay(TimeToSleepFromContext.Invoke(Data(instance)));
                return await Task.FromResult(ActivityExecutionResult.Next(instance));
            }

            await Task.Delay(_time);
            return await Task.FromResult(ActivityExecutionResult.Next(instance));
        }
    }
}
