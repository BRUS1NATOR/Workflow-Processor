using WorkflowProcessor.Activities;
using WorkflowProcessor.Activities.Basic;
using WorkflowProcessor.Core;
using WorkflowProcessor.Core.Connections;
using WorkflowProcessor.Persistance.Context;
using WorkflowProcessor.Persistance.Context.Json;

namespace WorkflowProcessor.Console.Examples
{
    [PolymorphicContext(typeof(Data5), "Data5")]
    public class Data5 : IContextData
    {
        public long Varialbe { get; set; } = 0;
    }
    public class ParallelGatewayExample : WorkflowBuilder<Data5>
    {
        public ParallelGatewayExample()
        {
            Name = "Example_Test_5";
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
            var logValue2 = Step<LogActivity<Data5>>(activity => activity.Log(context => "Log2: " + context.Data.Varialbe)).WithId("Log2");
            var logValue3 = Step<LogActivity<Data5>>(activity => activity.Log(context => "Log3: " + context.Data.Varialbe)).WithId("Log3");
            //
            var parallelGatewayStart = StepParallelGateway(x => x.Data.Varialbe == 0);
            var parallelGatewayClose = StepParallelGatewayClose(parallelGatewayStart);

            var sleepActivity = Step<SleepActivity<Data5>>(x => x.Sleep(5000));
            var endActivity = Step<EndActivity>();

            Scheme.Connections = new List<Connection>()
            {
                new Connection(start, parallelGatewayStart),
                    new ConditionalConnection<Data5, bool>(parallelGatewayStart, logValue2, true),
                    new ConditionalConnection<Data5, bool>(parallelGatewayStart, sleepActivity, true),
                new Connection(logValue2, parallelGatewayClose),
                new Connection(sleepActivity, logValue3),
                new Connection(logValue3, parallelGatewayClose),
                new Connection(parallelGatewayClose, endActivity)
            };

            return base.Build();
        }
    }
}
