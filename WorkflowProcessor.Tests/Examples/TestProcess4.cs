using WorkflowProcessor.Activities;
using WorkflowProcessor.Activities.Basic;
using WorkflowProcessor.Core;
using WorkflowProcessor.Core.Connections;
using WorkflowProcessor.Persistance.Context;
using WorkflowProcessor.Persistance.Context.Json;
using WorkflowProcessor.Tests.Examples;

namespace WorkflowProcessor.Console.Examples
{
    [PolymorphicContext(typeof(TestProcess4_Data), "TestProcess4_Data")]
    public class TestProcess4_Data : IContextData
    {
        public long Varialbe { get; set; } = 0;
    }
    public class TestProcess4 : WorkflowBuilder<TestProcess4_Data>
    {
        public TestProcess4()
        {
            Name = TestConsts.PROCESS_4;
            Version = 1;
        }
        public override Workflow Build()
        {
            //
            var start = StepStart(x =>
            {
                x.Code(con =>
                {
                    con.WorkflowInstance.Name = "Subprocess example";
                });
            });
            //
            var logValue = Step<LogActivity<TestProcess4_Data>>(activity => activity.Log(context => "Log1: " + context.Data.Varialbe));
            var logValue2 = Step<LogActivity<TestProcess4_Data>>(activity => activity.Log(context => "Log2: " + context.Data.Varialbe));
            //
            var subProcess = Step<SubprocessActivity<TestProcess4_Data, TestProcess2, TestProcess2_Data>>(activity =>
            {
                activity.SetBookmark(true);
                activity.SetContextData(x =>
                {
                    return new TestProcess2_Data()
                    {
                        Varialbe = 1
                    };
                });
            });

            var endActivity = Step<EndActivity>();

            Scheme.Connections = new List<Connection>()
            {
                new Connection(start, logValue),
                new Connection(logValue, subProcess),
                new Connection(subProcess, logValue2),
                new Connection(logValue2, endActivity)
            };

            return base.Build();
        }
    }
}
