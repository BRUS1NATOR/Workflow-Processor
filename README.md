# Workflow Processor

Project insipred by [WorkflowCore](https://github.com/danielgerlag/workflow-core) and [ELSA](https://github.com/elsa-workflows/elsa-core) that allows you to build BPMN processes with C# code. 
Pros:
 * Steps (tasks) support DI in class constructor. (unlike ELSA)
 * Build connections with connectors (just like in ELSA, unlike WorkflowCore)

## Example
![Visualization](images/workflow_example.drawio.png)
### Define context
```
// Context data of process
public class Data : IContextData
{
    public long Varialbe { get; set; }
}
```
### Define process
```
public class TestProcess : WorkflowBuilder<Data>
{
    // Метаданные процесса
    public TestProcess()
    {
        Name = "Test Process";
        Version = 1;
    }
    
    public override Workflow Build()
    {
        // Схема процесса
        ...
        return base.Build();
    }
}
```


### Define process scheme
```
public override Workflow Build()
{
    // Scheme
    // Steps
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
    
    // Conntections between steps
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
```