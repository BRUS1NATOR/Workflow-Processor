using Microsoft.AspNetCore.Mvc;
using WorkflowProcessor.API.Controllers;
using WorkflowProcessor.Services;

[ApiController]
[Route("/api/[controller]/[action]")]
public class ExecutionPointController : ControllerBase
{
    private readonly ILogger<BookmarkController> _logger;
    private readonly WorkflowContext _dbContext;
    private readonly IWorkflowUserService _workflowUserService;

    public ExecutionPointController(ILogger<BookmarkController> logger, WorkflowContext dbContext, WorkflowBookmarkService bookmarkService, IWorkflowUserService userService)
    {
        _logger = logger;
        _dbContext = dbContext;
        _workflowUserService = userService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<WorkflowExecutionPoint>> Get([FromQuery] int workflowInstance)
    {
        return Ok(_dbContext.WorkflowExecutionPoints.Where(x => x.WorkflowInstanceId == workflowInstance));
    }
}