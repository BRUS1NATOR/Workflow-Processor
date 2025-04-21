using WorkflowProcessor.Activities.Basic;
using WorkflowProcessor.Core;
using WorkflowProcessor.Core.ExecutionResults;
using WorkflowProcessor.Core.WorkflowElement;
using WorkflowProcessor.Persistance.Context;

namespace WorkflowProcessor.Activities
{
    public class SubprocessActivity<TContextData, TProcess> : WorkflowElement<TContextData>, IBlockingActivity
        where TContextData : IContextData, new()
        where TProcess : WorkflowBuilder, new()
    {
        protected bool isBookmark = true;
        protected WorkflowExecutor _workflowExecutor;

        public SubprocessActivity(WorkflowExecutor workflowExecutor)
        {
            _workflowExecutor = workflowExecutor;
        }

        public void SetBookmark(bool bookmark)
        {
            isBookmark = bookmark;
        }

        public override async Task<ActivityExecutionResult> ExecuteAsync(IWorkflowInstance instance)
        {
            var child = await _workflowExecutor.StartProcessAsync<TProcess>(parent: instance);
            if (isBookmark)
            {
                return ActivityExecutionBlockingResult.Bookmark(instance, nameof(TProcess), child.Id);
            }
            return ActivityExecutionResult.Next(instance);
        }
    }

    public class SubprocessActivity<TContextData, TProcess, TProcessContext> : SubprocessActivity<TContextData, TProcess>
        where TContextData : IContextData, new()
        where TProcess : WorkflowBuilder<TProcessContext>, new()
        where TProcessContext : IContextData, new()
    {
        protected Func<Context<TContextData>, TProcessContext>? GetContextData;

        public SubprocessActivity(WorkflowExecutor workflowExecutor) : base(workflowExecutor)
        {
        }

        public void SetContextData(Func<Context<TContextData>, TProcessContext> func)
        {
            GetContextData = func;
        }

        public override async Task<ActivityExecutionResult> ExecuteAsync(IWorkflowInstance instance)
        {
            IWorkflowInstance? child = null;
            if (GetContextData is not null)
            {
                var data = GetContextData(Data(instance));
                child = await _workflowExecutor.StartProcessAsync<TProcess, TProcessContext>(data, parent: instance);
            }
            else
            {
                child = await _workflowExecutor.StartProcessAsync<TProcess, TProcessContext>(default, parent: instance);
            }
            if (isBookmark)
            {
                return ActivityExecutionBlockingResult.Bookmark(instance, nameof(TProcess), child.Id);
            }
            return ActivityExecutionResult.Next(instance);
        }
    }
}
