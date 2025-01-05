using WorkflowProcessor.Activities.Basic;
using WorkflowProcessor.Core;
using WorkflowProcessor.Core.Results;
using WorkflowProcessor.Persistance.Context;

namespace WorkflowProcessor.Activities
{
    public class UserActivity : Gateway<string>, IBlockingActivity
    {
        public int UserId { get; set; }
        public string TaskName { get; set; } = "Задача";


        public void SetUserId(int userId)
        {
            UserId = userId;
        }

        public override async Task<WorkflowResult> ExecuteAsync(IWorkflowInstance context)
        {
            return await Task.FromResult(WorkflowUserResult.UserTask(context, UserId, TaskName));
        }

    }

    public class UserActivity<TContext> : UserActivity where TContext : IContextData, new()
    {

    }
}