using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkflowProcessor.Core;
using WorkflowProcessor.Services;

namespace WorkflowProcessor.API.Controllers
{
    [ApiController]
    [Route("/api/[controller]/[action]")]
    public class WorkflowController : ControllerBase
    {
        private readonly ILogger<WorkflowController> _logger;

        private readonly IWorkflowUserService _workflowUserService;
        private readonly WorkflowContext _dbContext;
        private readonly WorkflowExecutor _workflowManager;
        private readonly WorkflowStorage _workflowStorage;

        public WorkflowController(ILogger<WorkflowController> logger, IWorkflowUserService workflowUserService, WorkflowContext dbContext, WorkflowExecutor workflowManager, WorkflowStorage workflowStorage)
        {
            _logger = logger;
            _workflowUserService = workflowUserService;
            _dbContext = dbContext;
            _workflowManager = workflowManager;
            _workflowStorage = workflowStorage;
        }

        [HttpGet]
        public IEnumerable<Workflow> Get()
        {
            return _workflowStorage.Workflows;
        }

        [HttpGet("{id}")]
        public ActionResult<Workflow> Get(string id)
        {
            var workflow = _workflowStorage.GetWorkflow(id, null);
            if (workflow is null)
            {
                return NotFound();
            }
            return Ok(workflow);
        }

        [HttpPost]
        //[Authorize]
        public async Task<ActionResult<WorkflowInstance>> Start([FromBody] WorkflowIdentifier workflowInfo)
        {
            var workflow = _workflowStorage.GetWorkflow(workflowInfo);
            if (workflow is null)
            {
                return NotFound();
            }
            if (!workflow.IsAllowedToRunFromWeb)
            {
                return NotFound("Process is not allowed to be started from Web");
            }
            var userId = 1;
            //var userId = _workflowUserService.GetUserId(HttpContext.User);
            //if (userId is null)
            //{
            //    return Unauthorized();
            //}
            var result = await _workflowManager.StartProcessAsync(workflow, userId);
            return Ok(result);
        }
    }
}
