using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkflowProcessor.API.Extensions;
using WorkflowProcessor.API.Models;
using WorkflowProcessor.Core.ExecutionResults;
using WorkflowProcessor.Services;

namespace WorkflowProcessor.API.Controllers
{
    [ApiController]
    [Route("/api/[controller]/[action]")]
    [Authorize]
    public class BookmarkController : ControllerBase
    {
        private readonly ILogger<BookmarkController> _logger;
        private readonly WorkflowContext _dbContext;
        private readonly WorkflowBookmarkService _bookmarkService;
        private readonly IWorkflowUserService _workflowUserService;

        public BookmarkController(ILogger<BookmarkController> logger, WorkflowContext dbContext, WorkflowBookmarkService bookmarkService, IWorkflowUserService userService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _bookmarkService = bookmarkService;
            _workflowUserService = userService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WorkflowBookmark>> Get(int id)
        {
            var userTask = await _dbContext.WorkflowBookmarks
                .Include(x => x.UserTasks)
                .Include(x => x.WorkflowExecutionPoint)
                .ThenInclude(x => x.WorkflowInstance)
                .Where(x => x.Id == id).FirstOrDefaultAsync();

            if (userTask is null)
            {
                return NotFound();
            }
            return Ok(userTask);
        }

        [HttpGet]
        //[Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<WorkflowBookmark>>> Get([FromQuery] WorkflowBookmarkFilter filter)
        {
            return await _dbContext.WorkflowBookmarks
                .Include(x => x.UserTasks)
                .Include(x => x.WorkflowExecutionPoint)
                .ThenInclude(x => x.WorkflowInstance)
                    .Where(x => x.UserTasks.Any(t => filter.UserId == null || t.UserId == filter.UserId))
                    .WhereIf(filter.Status != null, x => x.Status == filter.Status)
                    .WhereIf(filter.Type != null, x => x.Type == filter.Type)
                    .WhereIf(filter.Id != null, x => x.Id == filter.Id)
                    .WhereIf(filter.Name != null, x => x.Name != null && x.Name.Contains(filter.Name))
                .ToListAsync();
        }

        [HttpGet]
        //[Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<WorkflowBookmark>>> GetActive()
        {
            return await _dbContext.WorkflowBookmarks
                .Include(x => x.UserTasks)
                .Include(x => x.WorkflowExecutionPoint)
                .ThenInclude(x => x.WorkflowInstance)
                .Where(x => x.Status == Core.Enums.WorkflowBookmarkStatus.Active)
                .ToListAsync();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkflowBookmark>>> GetCurrentUserActive()
        {
            var userid = _workflowUserService.GetUserId(User);

            if (userid is null)
            {
                return Unauthorized();
            }

            return Ok(await _dbContext.WorkflowBookmarks
                .Include(x => x.UserTasks)
                .Include(x => x.WorkflowExecutionPoint)
                .ThenInclude(x => x.WorkflowInstance)
                .Where(x =>
                    x.Type == Core.Enums.WorkflowBookmarkType.UserTask &&
                    x.UserTasks.Any(x => x.UserId == userid) &&
                    x.Status == Core.Enums.WorkflowBookmarkStatus.Active)
                .ToListAsync());
        }

        [HttpGet("{workflowInstance}")]
        public async Task<ActionResult<IEnumerable<WorkflowBookmark>>> GetCurrentUserActiveByWorkflowInstance(int workflowInstance)
        {
            var userid = _workflowUserService.GetUserId(User);

            if (userid is null)
            {
                return Unauthorized();
            }

            return Ok(await _dbContext.WorkflowBookmarks
                .Include(x => x.UserTasks)
                .Include(x => x.WorkflowExecutionPoint)
                .ThenInclude(x => x.WorkflowInstance)
                .Where(x =>
                    x.UserTasks.Any(x => x.UserId == userid) &&
                    x.Status == Core.Enums.WorkflowBookmarkStatus.Active &&
                    x.WorkflowExecutionPoint.WorkflowInstanceId == workflowInstance)
                .ToListAsync());
        }

        [HttpGet("{workflowInstanceId}")]
        public async Task<IEnumerable<WorkflowBookmark>> GetByWorkflowInstance(int workflowInstanceId)
        {
            return await _dbContext.WorkflowBookmarks
                .Include(x => x.UserTasks)
                .Include(x => x.WorkflowExecutionPoint)
                .Where(x => x.WorkflowExecutionPoint.WorkflowInstanceId == workflowInstanceId).ToListAsync();
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<WorkflowExecutionResult>> Complete([FromRoute] int id, [FromBody] string nextStepId)
        {
            var userid = _workflowUserService.GetUserId(User);

            if (userid is null)
                return Unauthorized();

            var bookmark = _dbContext.WorkflowBookmarks
                .Include(x => x.UserTasks)
                .Include(x => x.WorkflowExecutionPoint)
                .ThenInclude(x => x.WorkflowInstance)
                .FirstOrDefault(x => x.Id == id);

            if (bookmark is null)
            {
                return NotFound();
            }
            if (bookmark.Status != Core.Enums.WorkflowBookmarkStatus.Active)
            {
                return BadRequest($"Bookmark status: {bookmark.Status} is not allowed");
            }
            if (!bookmark.UserTasks.Any(x => x.UserId == userid))
            {
                return BadRequest($"User is not allowed to complete bookmark");
            }
            ActivityExecutionResultWithValue? resultWithValue = null;
            if (!string.IsNullOrEmpty(nextStepId))
            {
                resultWithValue = new ActivityExecutionResultWithValue()
                {
                    Value = nextStepId
                };
            }

            var wfExecutionResult = await _bookmarkService.BookmarkCompleteAsync(bookmark, resultWithValue);
            return Ok(wfExecutionResult);
        }
    }
}