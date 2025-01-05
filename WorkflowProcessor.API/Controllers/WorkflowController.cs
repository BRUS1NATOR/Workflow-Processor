using MassTransitExample.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkflowProcessor.Core;
using WorkflowProcessor.Services;

namespace WorkflowProcessor.API.Controllers
{
    [ApiController]
    [Route("/api/[controller]/[action]")]
    public class WorkflowController : ControllerBase
    {
        private readonly ILogger<BookmarkController> _logger;
        private readonly WorkflowContext _dbContext;
        private readonly WorkflowExecutor _workflowManager;
        private readonly WorkflowStorage _workflowStorage;

        public WorkflowController(ILogger<BookmarkController> logger, WorkflowContext dbContext, WorkflowExecutor workflowManager, WorkflowStorage workflowStorage)
        {
            _logger = logger;
            _dbContext = dbContext;
            _workflowManager = workflowManager;
            _workflowStorage = workflowStorage;
        }

        [HttpGet]
        public IEnumerable<Workflow> Get()
        {
            return _workflowStorage.GetWorkflowList();
        }

        [HttpPost("/start")]
        public async Task<IActionResult> StartWorkflow(string workflowId, int version)
        {
            var workflow = _workflowStorage.GetWorkflow(workflowId, version);
            if (workflow is null)
            {
                return NotFound();
            }
            await _workflowManager.StartProcessAsync(workflow);
            return Ok();
        }
    }
}
