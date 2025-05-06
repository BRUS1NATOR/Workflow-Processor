using System.Text.Json.Serialization;

namespace WorkflowProcessor.Core.ExecutionResults
{

    public class WorkflowExecutionResult : IWorkflowResult
    {
        [JsonPropertyName("isSuccess")]
        public bool IsSuccess { get; set; } = true;

        [JsonPropertyName("message")]
        public string Message { get; set; }

        public WorkflowExecutionResult()
        {

        }

        public WorkflowExecutionResult(bool isSuccess, string message = "")
        {
            IsSuccess = isSuccess;
            Message = message;
        }
    }
}
