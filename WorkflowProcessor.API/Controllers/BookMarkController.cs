using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkflowProcessor.Core.Results;
using WorkflowProcessor.Services;

namespace WorkflowProcessor.API.Controllers
{
    [ApiController]
    [Route("/api/[controller]/[action]")]
    public class BookmarkController : ControllerBase
    {
        private readonly ILogger<BookmarkController> _logger;
        private readonly WorkflowContext _dbContext;
        private readonly WorkflowBookmarkService _bookmarkService;

        public BookmarkController(ILogger<BookmarkController> logger, WorkflowContext dbContext, WorkflowBookmarkService bookmarkService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _bookmarkService = bookmarkService;
        }

        [HttpGet]
        public async Task<IEnumerable<WorkflowBookmark>> Get()
        {
            return await _dbContext.WorkflowBookmarks.ToListAsync();
        }

        [HttpGet]
        public async Task<IEnumerable<WorkflowBookmark>> GetActive()
        {
            return await _dbContext.WorkflowBookmarks.Where(x => x.Status == Core.Enums.WorkflowBookmarkStatus.Active).ToListAsync();
        }

        [HttpPost("{id}/complete")]
        public async Task<IActionResult> UserBookmark(int id, string nextStepId)
        {
            var bookmark = _dbContext.WorkflowBookmarks
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
            await _bookmarkService.BookmarkCompleteAsync(bookmark, new WorkflowUserResult()
            {
                NextStepId = nextStepId
            });
            return Ok();
        }
    }
}
