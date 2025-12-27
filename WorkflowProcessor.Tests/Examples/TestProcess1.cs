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
    [PolymorphicContext(typeof(TestProcess1_Data), "TestProcess1_Data")]
    public class TestProcess1_Data : IContextData
    {
        public string NameVariable { get; set; } = "";

        // Variable for test
        public bool IsStartStepCompleted { get; set; } = false;
    }

    public class TestProcess1 : WorkflowBuilder<TestProcess1_Data>
    {
        public TestProcess1()
        {
            Name = TestConsts.PROCESS_1;
            Version = 1;
        }

        public override Workflow Build()
        {
            //
            var start = Step<StartActivity<TestProcess1_Data>>(x => x.Code(exec => exec.Data.IsStartStepCompleted = true));
            var helloWorld = Step<LogActivity>(activity => activity.Log("Hello world!"));

            var setVariableValue = Step<CodeActivity<TestProcess1_Data>>((activity) =>
            {
                activity.Code(context => { context.Data.NameVariable = "Alex"; });
            });

            var statementIsTrue = Step<LogActivity>(activity => activity.Log("Your name is Alex!"));
            var statementIsFalse = Step<LogActivity>(activity => activity.Log("Your name is not Alex!"));

            var ifStatement = Step<If<TestProcess1_Data>>(activity =>
            {
                activity.SetCondition(context => context.Data.NameVariable == "Alex");
            });

            var helloUserActivity = Step<CodeActivity<TestProcess1_Data>>((activity) =>
            {
                activity.Code(x => { System.Console.WriteLine(x.Data.NameVariable); });
            });
            var endActivity = Step<EndActivity>();

            Scheme.Connections = new () {
                    new Connection(start, helloWorld),
                    new Connection(helloWorld, setVariableValue),
                    new Connection(setVariableValue, helloUserActivity),

                    new Connection(helloUserActivity, ifStatement),
                        new ConditionalConnection<TestProcess1_Data, bool>(ifStatement, statementIsTrue, true),
                        new ConditionalConnection<TestProcess1_Data, bool>(ifStatement, statementIsFalse, false),

                    new Connection(statementIsTrue, endActivity),
                    new Connection(statementIsFalse, endActivity)
            };

            return base.Build();
        }
    }
}
