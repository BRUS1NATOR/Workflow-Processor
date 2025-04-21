using WorkflowProcessor.Activities;
using WorkflowProcessor.Activities.Basic;
using WorkflowProcessor.Core;
using WorkflowProcessor.Core.Connections;
using WorkflowProcessor.Persistance.Context;
using WorkflowProcessor.Persistance.Context.Json;

namespace WorkflowProcessor.Console.Examples
{
    [PolymorphicContext(typeof(Data4), "Data4")]
    public class Data4 : IContextData
    {
        public long Varialbe { get; set; } = 0;
    }
    public class TestProcess4 : WorkflowBuilder<Data4>
    {
        public TestProcess4()
        {
            Name = "Example_Test_4";
            Version = 1;
        }
        public override Workflow Build()
        {
            //
            var start = StepStart(x =>
            {
                x.Code(con =>
                {
                    con.WorkflowInstance.Name = "Привет мир!";
                });
            });
            //
            var logValue = Step<LogActivity<Data4>>(activity => activity.Log(context => "Log1: " + context.Data.Varialbe));
            var logValue2 = Step<LogActivity<Data4>>(activity => activity.Log(context => "Log2: " + context.Data.Varialbe));
            //
            var subProcess = Step<SubprocessActivity<Data4, TestProcess2, Data2>>(activity =>
            {
                activity.SetBookmark(true);
                activity.SetContextData(x =>
                {
                    return new Data2()
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
