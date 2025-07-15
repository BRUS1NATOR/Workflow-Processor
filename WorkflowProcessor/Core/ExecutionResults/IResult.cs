namespace WorkflowProcessor.Core.ExecutionResults
{
    public interface IResult
    {
        public ExecutionResultStatusCode StatusCode { get; }
        public bool IsSuccess { get; }
        public string Message { get; set; }
    }
    public interface IWorkflowResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
