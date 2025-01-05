namespace WorkflowProcessor.Core.Enums
{
    public enum WorkflowExecutionStepStatus
    {
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
        /// The workflow has faulted.
        /// </summary>
        Faulted,
    }
}
