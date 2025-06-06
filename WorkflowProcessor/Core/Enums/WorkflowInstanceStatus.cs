﻿namespace WorkflowProcessor.Core.Enums
{

    public enum WorkflowInstanceStatus
    {
        /// <summary>
        /// The workflow is pending execution.
        /// </summary>
        Pending,

        /// <summary>
        /// The workflow is currently executing.   
        /// </summary>
        Executing,

        /// <summary>
        /// The workflow is currently suspended and waiting for external stimuli to resume.   
        /// </summary>
        Suspended,

        /// <summary>
        /// The workflow completed successfully.
        /// </summary>
        Finished,

        /// <summary>
        /// The workflow was cancelled.
        /// </summary>
        Cancelled,

        /// <summary>
        /// The workflow has faulted.
        /// </summary>
        Faulted,
    }
}
