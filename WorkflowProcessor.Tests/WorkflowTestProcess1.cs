using WorkflowProcessor.Console.Examples;
using WorkflowProcessor.Core;
using WorkflowProcessor.Tests.Examples;
using WorkflowProcessor.Tests.Fixtures;
using Xunit.Abstractions;

namespace WorkflowProcessor.Tests
{
    public class WorkflowTestProcess1 : WorkflowTest
    {
        public WorkflowTestProcess1(WorkflowTestsFixture workflowFixture, ITestOutputHelper output) : base(workflowFixture, output)
        {
        }

        [Fact]
        public async Task Test1()
        {
            Workflow workflow = workflowFixture.WorkflowStorage.AddWorkflow<TestProcess1>();

            Assert.NotNull(workflow);
            Assert.Equal(TestConsts.PROCESS_1, workflow.Name);
            Assert.Equal(1, workflow.Version);

            var result = await workflowFixture.WorkflowExecutor.StartProcessAsync(workflow);
            output.WriteLine(result.Context.JsonData);
            //Wait for process to end
            await Task.Delay(3000);
            output.WriteLine(result.Context.JsonData);

            var contextData = (TestProcess1_Data)result.Context.DataObject!;
            Assert.True(contextData!.IsStartStepCompleted);
            output.WriteLine(result.Context.JsonData);

            output.WriteLine(workflowFixture.DbFixture.DbContext.WorkflowInstances.FirstOrDefault().Context.JsonData);
        }
    }
}
