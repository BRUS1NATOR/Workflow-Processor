﻿using WorkflowProcessor.Core;
using WorkflowProcessor.Core.ExecutionResults;
using WorkflowProcessor.Core.Step;
using WorkflowProcessor.Core.WorkflowElement;

namespace WorkflowProcessor.Activities.Basic
{
    [ActivityType(BaseAcitivityType.EndActivity)]
    public class EndActivity : WorkflowElement
    {
        public EndActivity()
        {
        }

        public override async Task<ActivityExecutionResult> ExecuteAsync(IWorkflowInstance instance)
        {
            return await Task.FromResult(ActivityExecutionResult.Finish(instance));
        }
    }
}
