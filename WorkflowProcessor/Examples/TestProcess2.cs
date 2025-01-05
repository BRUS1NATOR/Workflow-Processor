using WorkflowProcessor.Activities;
using WorkflowProcessor.Activities.Basic;
using WorkflowProcessor.Core;
using WorkflowProcessor.Core.Connections;
using WorkflowProcessor.Persistance.Context;
namespace MassTransitExample.Examples
{
    public class Data2 : IContextData
    {
        public long Varialbe { get; set; }
    }
    public class TestProcess2 : WorkflowBuilder<Data2>
    {
        public TestProcess2()
        {
            Name = "Test 2";
            Version = 1;
        }
        public override Workflow Build()
        {
            //
            var start = Step<StartActivity>();
            //
            var logValue = Step<LogActivity<Data2>>(activity => activity.Log(context => "Значение: " + context.Varialbe));
            //
            var increaseValueByOne = Step<CodeActivity<Data2>>(activity => activity.Code(context => { context.Varialbe++; }));
            var endCycle = Step<LogActivity>(activity => activity.Log("Значение >= 5"));
            var ifStatement = Step<If<Data2>>(activity =>
            {
                activity.SetCondition(context => context.Varialbe >= 5);
            });
            var endActivity = Step<EndActivity>();

            Connections = new List<Connection>()
                {
                    new Connection(start, logValue),
                    new Connection(logValue, ifStatement),
                        new ConditionalConnection<Data2, bool>(ifStatement, endCycle, true),
                        new ConditionalConnection<Data2, bool>(ifStatement, increaseValueByOne, false),
                    new Connection(increaseValueByOne, logValue),
                    new Connection(endCycle, endActivity)
                };

            return base.Build();
        }
    }
}