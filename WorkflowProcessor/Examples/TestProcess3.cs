using WorkflowProcessor.Activities;
using WorkflowProcessor.Activities.Basic;
using WorkflowProcessor.Core;
using WorkflowProcessor.Core.Connections;
using WorkflowProcessor.Persistance.Context;
namespace MassTransitExample.Examples
{
    public class Data3 : IContextData
    {
        public string Varialbe { get; set; } // = "ABC";
    }
    public class TestProcess3 : WorkflowBuilder<Data3>
    {
        public TestProcess3()
        {
            Name = "Test 3";
            Version = 1;
        }
        public override Workflow Build()
        {
            //
            var start = Step<StartActivity>(x => { });
            var logValue = Step<LogActivity<Data3>>(x => x.Log(context => context.Varialbe));
            var log1 = Step<LogActivity>(x => x.Log("Значение равно A"));
            var log2 = Step<LogActivity>(x => x.Log("Значение равно B"));
            var log3 = Step<LogActivity>(x => x.Log("Значение равно C"));
            var log4 = Step<LogActivity>(x => x.Log("Значение начинается с буквы А"));
            var log5 = Step<LogActivity>(x => x.Log("Значение любое"));
            var stringGateway = Step<Gateway<Data3, string>>(activity => activity.SetCondition(x => x.Varialbe));
            var endLogActivity = Step<EndActivity>();

            Connections = new List<Connection>()
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