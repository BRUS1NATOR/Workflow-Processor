using WorkflowProcessor.Core;
using WorkflowProcessor.Core.ExecutionResults;
using WorkflowProcessor.Core.WorkflowElement;
using WorkflowProcessor.Persistance.Context;

namespace WorkflowProcessor.Activities.Basic
{
    public interface IBlockingActivity : IWorkflowElement
    {
    }

    [ActivityType(BaseAcitivityType.BlockingActivity)]
    public class BlockingActivity : WorkflowElement, IBlockingActivity
    {
        private string? _bookMarkName;

        private Func<Context, string>? _bookMarkNameFunc;

        public void SetBookmarkName(string name)
        {
            _bookMarkName = name;
        }

        public void SetBookmarkName(Func<Context, string> name)
        {
            _bookMarkNameFunc = name;
        }

        private string GetBookmarkName(IWorkflowInstance wfInstance)
        {
            if (_bookMarkName is not null)
            {
                return _bookMarkName;
            }

            if (_bookMarkName is not null)
            {
                return _bookMarkNameFunc.Invoke(wfInstance.Context);
            }
            return string.Empty;
        }

        public override async Task<ActivityExecutionResult> ExecuteAsync(IWorkflowInstance wfInstance)
        {
            return await Task.FromResult(ActivityExecutionBlockingResult.Bookmark(wfInstance, GetBookmarkName(wfInstance)));
        }
    }

    [ActivityType(BaseAcitivityType.BlockingActivity)]
    public class BlockingActivity<TContextData> : WorkflowElement<TContextData>, IBlockingActivity
        where TContextData : IContextData, new()
    {
        protected Func<Context<TContextData>, string>? _bookmarkNameFunc;

        public void SetBookmarkName(Func<Context<TContextData>, string> bookmarkNameFunction)
        {
            _bookmarkNameFunc = bookmarkNameFunction;
        }

        private string GetBookmarkName(Context<TContextData> context)
        {
            if (_bookmarkNameFunc is not null)
            {
                return _bookmarkNameFunc.Invoke(context);
            }

            return Guid.NewGuid().ToString();
        }

        public override async Task<ActivityExecutionResult> ExecuteAsync(IWorkflowInstance wfInstance)
        {
            var bookmarkName = GetBookmarkName(Data(wfInstance));
            return await Task.FromResult(ActivityExecutionBlockingResult.Bookmark(wfInstance, bookmarkName));
        }
    }
}