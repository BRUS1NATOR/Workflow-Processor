﻿using System.Text.Json.Serialization;
using WorkflowProcessor.Activities;
using WorkflowProcessor.Activities.Basic;
using WorkflowProcessor.Activities.Gateways;
using WorkflowProcessor.Core;
using WorkflowProcessor.Core.Connections;
using WorkflowProcessor.Persistance.Context;
using WorkflowProcessor.Persistance.Context.Json;

namespace WorkflowProcessor.Console.Examples
{
    [PolymorphicContext(typeof(Data1), "Data1")]
    public class Data1 : IContextData
    {
        public string NameVariable { get; set; }
    }

    public class TestProcess1 : WorkflowBuilder<Data1>
    {
        public TestProcess1()
        {
            Name = "Example_Test_1";
            Version = 1;
        }

        public override Workflow Build()
        {
            //
            var start = Step<StartActivity>(x => { System.Console.WriteLine("START"); });
            var helloWorld = Step<LogActivity>(activity => activity.Log("Hello world!"));

            var setVariableValue = Step<CodeActivity<Data1>>((activity) =>
            {
                activity.Code(context => { context.Data.NameVariable = "Alex"; });
            });

            var statementIsTrue = Step<LogActivity>(activity => activity.Log("Your name is Alex!"));
            var statementIsFalse = Step<LogActivity>(activity => activity.Log("Your name is not Alex!"));

            var ifStatement = Step<If<Data1>>(activity =>
            {
                activity.SetCondition(context => context.Data.NameVariable == "Alex");
            });

            var helloUserActivity = Step<CodeActivity<Data1>>((activity) =>
            {
                activity.Code(x => { System.Console.WriteLine(x.Data.NameVariable); });
            });
            var endActivity = Step<EndActivity>(x => { System.Console.WriteLine("END"); });

            Scheme.Connections = new () {
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
