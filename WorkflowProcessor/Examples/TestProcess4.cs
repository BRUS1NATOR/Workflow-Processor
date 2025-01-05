using WorkflowProcessor.Activities;
using WorkflowProcessor.Activities.Basic;
using WorkflowProcessor.Core;
using WorkflowProcessor.Core.Connections;
namespace MassTransitExample.Examples
{
    public class TestProcess4 : WorkflowBuilder
    {
        public TestProcess4()
        {
            Name = "Test 4";
            Version = 1;
        }
        public override Workflow Build()
        {
            var start = Step<StartActivity>(x => { });
            //
            var log1 = Step<LogActivity>(x => x.Log("START"));
            var userActivity = Step<UserActivity>(x =>
            {
                x.SetUserId(9250);
            }
            ).WithId("userTask1");

            var log2 = Step<LogActivity>(x => x.Log("Пользователь завершил задачу log2!")).WithId("log2");
            var log3 = Step<LogActivity>(x => x.Log("Пользователь завершил задачу log3!")).WithId("log3");
            var endActivity = Step<EndActivity>();

            Connections = new List<Connection>()
                {
                    new Connection(start, log1),
                    new Connection(log1, userActivity),
                        new ConditionalConnection<string>(userActivity, log2, "log2"),
                        new ConditionalConnection<string>(userActivity, log3, "log3"),
                   new Connection(log2, endActivity),
                   new Connection(log3, endActivity)
                };

            return base.Build();
        }
    }
}