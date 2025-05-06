using WorkflowProcessor.Activities.Basic;
using WorkflowProcessor.Activities.Gateways;
using WorkflowProcessor.Core;
using WorkflowProcessor.Core.ExecutionResults;
using WorkflowProcessor.Core.Step;
using WorkflowProcessor.Persistance.Context;

namespace WorkflowProcessor.Activities
{
    [ActivityType(BaseAcitivityType.UserActivity)]
    public class UserActivity<TContextData> : ExclusiveGateway<TContextData, string>, IBlockingActivity
        where TContextData : IContextData, new()
    {
        public List<long> Users { get; set; } = new List<long>();

        //
        protected string? _bookMarkName;
        private Func<Context<TContextData>, string>? _bookMarkNameFunc;
        //
        private Func<Context<TContextData>, long[]>? _getUsersFunction;

        public void AddUsers(long[] usersId)
        {
            Users.AddRange(usersId);
        }

        public void AddUser(long userId)
        {
            Users.Add(userId);
        }


        public void AddUsers(Func<Context<TContextData>, long[]> getUsersFunction)
        {
            _getUsersFunction = getUsersFunction;
        }


        public override async Task<ActivityExecutionResult> ExecuteAsync(IWorkflowInstance context)
        {
            var data = Data(context);
            if (_getUsersFunction is not null)
            {
                Users.AddRange(_getUsersFunction.Invoke(data));
            }
            return await Task.FromResult(ActivityExecutionUserResult.UserTask(context, Users, GetBookmarkName(data)));
        }

        public void SetBookmarkName(string name)
        {
            _bookMarkName = name;
        }
        public void SetBookmarkName(Func<Context<TContextData>, string> name)
        {
            _bookMarkNameFunc = name;
        }

        protected string GetBookmarkName(Context<TContextData> data)
        {
            if (_bookMarkName is not null)
            {
                return _bookMarkName;
            }

            if (_bookMarkNameFunc is not null)
            {
                return _bookMarkNameFunc.Invoke(data);
            }
            return string.Empty;
        }
    }
}