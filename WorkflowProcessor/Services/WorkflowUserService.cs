using System.Security.Claims;

namespace WorkflowProcessor.Services
{
    public class WorkflowUserService : IWorkflowUserService
    {
        public WorkflowUserService() { }

        public long? GetUserId(ClaimsPrincipal? claims)
        {
            if(claims is null)
            {
                return null;
            }

            var stringValue = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (stringValue is null)
            {
                return null;
            }
            if(Int64.TryParse(stringValue, out long result))
            {
                return result;
            }
            return null;
        }
    }

    public class WorkflowUserServiceExample : IWorkflowUserService
    {
        public WorkflowUserServiceExample() { }

        public long? GetUserId(ClaimsPrincipal httpContext)
        {
            return 1;
        }
    }
}
