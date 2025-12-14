using WorkflowProcessor.Activities;
using WorkflowProcessor.Activities.Gateways;
using WorkflowProcessor.Core;
using WorkflowProcessor.Core.Connections;
using WorkflowProcessor.Persistance.Context;
using WorkflowProcessor.Persistance.Context.Json;
using WorkflowProcessor.Tests.Examples;

namespace WorkflowProcessor.Console.Examples
{
    [PolymorphicContext(typeof(Data3), "Data3")]

    public class Data3 : IContextData
    {
        public string Varialbe { get; set; } // = "ABC";
    }
    public class TestProcess3 : WorkflowBuilder<Data3>
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
            var logValue = Step<LogActivity<Data3>>(x => x.Log(context => context.Data.Varialbe));
            var log1 = Step<LogActivity>(x => x.Log("Variable value equals \"A\""));
            var log2 = Step<LogActivity>(x => x.Log("Variable value equals \"B\""));
            var log3 = Step<LogActivity>(x => x.Log("Variable value equals \"C\""));
            var log4 = Step<LogActivity>(x => x.Log("Variable value starts with \"A\""));
            var log5 = Step<LogActivity>(x => x.Log("Any variable value"));
            var stringGateway = Step<ExclusiveGateway<Data3, string>>(activity => activity.SetCondition(x => x.Data.Varialbe));
            var endLogActivity = StepEnd();

            Scheme.Connections = new List<Connection>()
                {
                    new Connection(start, logValue),
                    new Connection(logValue, stringGateway),
                        new ConditionalConnection<Data3, string>(stringGateway, log1, "A"),
                        new ConditionalConnection<Data3, string>(stringGateway, log2, "B"),
                        new ConditionalConnection<Data3, string>(stringGateway, log3, "C"),
                        new ConditionalConnection<Data3, string>(stringGateway, log4, x => x == null ? false : x.StartsWith("A")),
                        //
                        new ConditionalConnection<Data3, string>(stringGateway, log5, x => true), // переход по умолчанию варинт N1
                        new Connection(stringGateway, log5),    // переход по умолчанию вариант N2
                    new Connection(log1, endLogActivity)
                };

            return base.Build();
        }
    }
}