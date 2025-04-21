namespace WorkflowProcessor.Core.ExecutionResults
{
    public interface IResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
