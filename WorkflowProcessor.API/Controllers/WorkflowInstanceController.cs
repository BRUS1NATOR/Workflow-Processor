using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkflowProcessor.API.Extensions;
using WorkflowProcessor.API.Models;
using WorkflowProcessor.Core;
using WorkflowProcessor.Services;

namespace WorkflowProcessor.API.Controllers
{
    [ApiController]
    [Route("/api/[controller]/[action]")]
    public class WorkflowInstanceController : ControllerBase
    {
        private readonly ILogger<WorkflowController> _logger;

        private readonly IWorkflowUserService _workflowUserService;
        private readonly WorkflowContext _dbContext;
        private readonly WorkflowExecutor _workflowManager;
        private readonly WorkflowStorage _workflowStorage;

        public WorkflowInstanceController(ILogger<WorkflowController> logger, IWorkflowUserService workflowUserService, WorkflowContext dbContext, WorkflowExecutor workflowManager, WorkflowStorage workflowStorage)
        {
            _logger = logger;
            _workflowUserService = workflowUserService;
            _dbContext = dbContext;
            _workflowManager = workflowManager;
            _workflowStorage = workflowStorage;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WorkflowInstance>> Get(long id)
        {
            var wfInstance = await _dbContext.WorkflowInstances
                .Include(x => x.Context)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (wfInstance is null)
            {
                return NotFound();
            }
            return Ok(wfInstance);
        }

        [HttpGet]
        //[Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<WorkflowInstance>>> Get([FromQuery] WorkflowInstanceFilter filter)
        {
            return await _dbContext.WorkflowInstances
                .Include(x => x.Context)
                    .WhereIf(filter.Id != null, x => x.Id == filter.Id)
                    .WhereIf(filter.Name != null, x => x.Name != null && x.Name.Contains(filter.Name!))
                .ToListAsync();
        }
    }
}
