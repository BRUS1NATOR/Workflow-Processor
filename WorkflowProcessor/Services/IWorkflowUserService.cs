using System.Security.Claims;

namespace WorkflowProcessor.Services
{
    public interface IWorkflowUserService
    {
        long? GetUserId(ClaimsPrincipal? claims);
    }
}