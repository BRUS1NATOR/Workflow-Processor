using System.Text.Json.Serialization;
using WorkflowProcessor.Activities;
using WorkflowProcessor.Activities.Basic;
using WorkflowProcessor.Core;
using WorkflowProcessor.Core.Connections;
using WorkflowProcessor.Core.WorkflowElement;
using WorkflowProcessor.Persistance.Context;

namespace MassTransitExample.Examples
{
    public class Data1 : IContextData
    {
        public string NameVariable { get; set; }
    }

    public class TestProcess1 : WorkflowBuilder<Data1>
    {
        public TestProcess1()
        {
            Name = "Test 1";
            Version = 1;
        }

        public override Workflow Build()
        {
            //
            var start = Step<StartActivity>(x => { Console.WriteLine("START"); });
            var helloWorld = Step<LogActivity>(activity => activity.Log("Hello world!"));

            var setVariableValue = Step<CodeActivity<Data1>>((activity) =>
            {
                activity.Code(context => { context.NameVariable = "Alex"; });
            });

            var statementIsTrue = Step<LogActivity>(activity => activity.Log("Your name is Alex!"));
            var statementIsFalse = Step<LogActivity>(activity => activity.Log("Your name is not Alex!"));

            var ifStatement = Step<If<Data1>>(activity =>
            {
                activity.SetCondition(context => context.NameVariable == "Alex");
            });

            var helloUserActivity = Step<CodeActivity<Data1>>((activity, context) =>
            {
                activity.Code(x => { Console.WriteLine(x.NameVariable); });
            });
            var endActivity = Step<EndActivity>(x => { Console.WriteLine("END"); });

            Connections = new () {
                    new Connection(start, helloWorld),
                    new Connection(helloWorld, setVariableValue),
                    new Connection(setVariableValue, helloUserActivity),

                    new Connection(helloUserActivity, ifStatement),
                        new ConditionalConnection<Data1, bool>(ifStatement, statementIsTrue, true),
                        new ConditionalConnection<Data1, bool>(ifStatement, statementIsFalse, false),

                    new Connection(statementIsTrue, endActivity),
                    new Connection(statementIsFalse, endActivity)
            };

            return base.Build();
        }
    }
}
