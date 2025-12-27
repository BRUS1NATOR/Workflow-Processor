using WorkflowProcessor.Activities;
using WorkflowProcessor.Activities.Basic;
using WorkflowProcessor.Activities.Gateways;
using WorkflowProcessor.Core;
using WorkflowProcessor.Core.Connections;
using WorkflowProcessor.Persistance.Context;
using WorkflowProcessor.Persistance.Context.Json;
using WorkflowProcessor.Tests.Examples;

namespace WorkflowProcessor.Console.Examples
{
    [PolymorphicContext(typeof(TestProcess2_Data), "TestProcess2_Data")]
    public class TestProcess2_Data : IContextData
    {
        public long Varialbe { get; set; } = 0;
    }
    public class TestProcess2 : WorkflowBuilder<TestProcess2_Data>
    {
        public TestProcess2()
        {
            Name = TestConsts.PROCESS_1;
            Version = 1;
            IsAllowedToRunFromWeb = true;
        }

        public override Workflow Build()
        {
            //
            var start = Step<StartActivity>();
            //
            var logValue = Step<LogActivity<TestProcess2_Data>>(activity => activity.Log(context => "Variable value: " + context.Data.Varialbe));
            //
            var increaseValueByOne = Step<CodeActivity<TestProcess2_Data>>(activity => activity.Code(context => { context.Data.Varialbe++; }));
            var endCycle = Step<LogActivity>(activity => activity.Log("Variable value >= 5"));
            var ifStatement = Step<If<TestProcess2_Data>>(activity =>
            {
                activity.SetCondition(context => context.Data.Varialbe >= 5);
            });
            var endActivity = Step<EndActivity>();

            Scheme.Connections = new List<Connection>()
                {
                    new Connection(start, logValue),
                    new Connection(logValue, ifStatement),
                        new ConditionalConnection<TestProcess2_Data, bool>(ifStatement, endCycle, true),
                        new ConditionalConnection<TestProcess2_Data, bool>(ifStatement, increaseValueByOne, false),
                    new Connection(increaseValueByOne, logValue),
                    new Connection(endCycle, endActivity)
                };

            return base.Build();
        }
    }
}