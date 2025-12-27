using WorkflowProcessor.Tests.Fixtures;
using Xunit.Abstractions;

namespace WorkflowProcessor.Tests
{
    public abstract class WorkflowTest : IClassFixture<WorkflowTestsFixture>
    {
        protected readonly ITestOutputHelper output;
        protected readonly WorkflowTestsFixture workflowFixture;
        public WorkflowTest(WorkflowTestsFixture workflowFixture, ITestOutputHelper output)
        {
            this.workflowFixture = workflowFixture;
            this.output = output;
        }
    }
}