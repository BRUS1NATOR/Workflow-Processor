using WorkflowProcessor.Activities;
using WorkflowProcessor.Activities.Gateways;
using WorkflowProcessor.Core;
using WorkflowProcessor.Core.Connections;
using WorkflowProcessor.Persistance.Context;
using WorkflowProcessor.Persistance.Context.Json;
using WorkflowProcessor.Tests.Examples;

namespace WorkflowProcessor.Console.Examples
{
    [PolymorphicContext(typeof(TestProcess3_Data), "TestProcess3_Data")]
    public class TestProcess3_Data : IContextData
    {
        public string Varialbe { get; set; } // = "ABC";
    }
    public class TestProcess3 : WorkflowBuilder<TestProcess3_Data>
    {
        public TestProcess3()
        {
            Name = TestConsts.PROCESS_1;
            Version = 1;
        }
        public override Workflow Build()
        {
            //
            var start = StepStart();
            var logValue = Step<LogActivity<TestProcess3_Data>>(x => x.Log(context => context.Data.Varialbe));
            var log1 = Step<LogActivity>(x => x.Log("Variable value equals \"A\""));
            var log2 = Step<LogActivity>(x => x.Log("Variable value equals \"B\""));
            var log3 = Step<LogActivity>(x => x.Log("Variable value equals \"C\""));
            var log4 = Step<LogActivity>(x => x.Log("Variable value starts with \"A\""));
            var log5 = Step<LogActivity>(x => x.Log("Any variable value"));
            var stringGateway = Step<ExclusiveGateway<TestProcess3_Data, string>>(activity => activity.SetCondition(x => x.Data.Varialbe));
            var endLogActivity = StepEnd();

            Scheme.Connections = new List<Connection>()
                {
                    new Connection(start, logValue),
                    new Connection(logValue, stringGateway),
                        new ConditionalConnection<TestProcess3_Data, string>(stringGateway, log1, "A"),
                        new ConditionalConnection<TestProcess3_Data, string>(stringGateway, log2, "B"),
                        new ConditionalConnection<TestProcess3_Data, string>(stringGateway, log3, "C"),
                        new ConditionalConnection<TestProcess3_Data, string>(stringGateway, log4, x => x == null ? false : x.StartsWith("A")),
                        //
                        new ConditionalConnection<TestProcess3_Data, string>(stringGateway, log5, x => true), // переход по умолчанию варинт N1
                        new Connection(stringGateway, log5),    // переход по умолчанию вариант N2
                    new Connection(log1, endLogActivity)
                };

            return base.Build();
        }
    }
}