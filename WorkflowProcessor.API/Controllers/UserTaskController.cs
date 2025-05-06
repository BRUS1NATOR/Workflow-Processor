using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkflowProcessor.API.Models;
using WorkflowProcessor.Services;

namespace WorkflowProcessor.API.Controllers
{
    [ApiController]
    [Route("/api/[controller]/[action]")]
    //[Authorize]
    public class UserTaskController : ControllerBase
    {
        private readonly ILogger<BookmarkController> _logger;
        private readonly WorkflowContext _dbContext;
        private readonly WorkflowBookmarkService _bookmarkService;
        private readonly IWorkflowUserService _workflowUserService;

        public UserTaskController(ILogger<BookmarkController> logger, WorkflowContext dbContext, WorkflowBookmarkService bookmarkService, IWorkflowUserService userService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _bookmarkService = bookmarkService;
            _workflowUserService = userService;
        }

        [HttpPost]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> ReplaceExecutor([FromBody] ReplaceExecutor replaceExecutor)
        {
            var bookmark = _dbContext.WorkflowBookmarks
                .Include(x => x.UserTasks)
                .FirstOrDefault(x => x.Id == replaceExecutor.BookmarkId);

            if (bookmark is null)
            {
                return NotFound();
            }
            if (bookmark.Status != Core.Enums.WorkflowBookmarkStatus.Active)
            {
                return BadRequest($"Bookmark status: {bookmark.Status} is not allowed");
            }
            var userTask = bookmark.UserTasks.FirstOrDefault(x => x.UserId == replaceExecutor.OldExecutor);

            if (userTask is null)
            {
                return BadRequest($"User task with user '{replaceExecutor.OldExecutor}' not found");
            }
            userTask.UserId = replaceExecutor.NewExecutor;
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
