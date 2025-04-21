namespace WorkflowProcessor.Core.ExecutionResults.Interfaces
{
    /// <summary>
    /// Result that blocks workflow instance flow and creates user task (bookmark)
    /// </summary>
    public interface IActivityExecutionUserResult : IActivityExecutionBlockingResult
    {
        /// <summary>
        /// Users that assigned to this user task (bookmark)
        /// </summary>
        public List<long> Users { get; set; }
    }
}