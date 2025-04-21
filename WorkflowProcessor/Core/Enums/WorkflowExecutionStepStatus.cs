namespace WorkflowProcessor.Core.Enums
{
    public enum WorkflowExecutionStepStatus
    {
        /// <summary>
        /// The step is currently executing.   
        /// </summary>
        Executing = 1,

        /// <summary>
        /// The step suspended and waiting to resume.   
        /// </summary>
        Suspended,

        /// <summary>
        /// The step completed successfully.
        /// </summary>
        Finished,

        /// <summary>
        /// The step has faulted.
        /// </summary>
        Faulted,
    }
}
