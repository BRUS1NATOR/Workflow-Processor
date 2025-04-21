using Microsoft.Extensions.Hosting;
using WorkflowProcessor.Console.Examples;
using WorkflowProcessor.Core;
using WorkflowProcessor.Services;

public class ProcessExampleWorker : BackgroundService
{
    readonly WorkflowExecutor _workflowManager;
    private readonly WorkflowStorage _workflowStorage;
    private readonly WorkflowContext _dbContext;
    private readonly WorkflowBookmarkService _bookmarkService;

    public ProcessExampleWorker(WorkflowExecutor workflowManager, WorkflowStorage workflowStorage, WorkflowContext dbContext, WorkflowBookmarkService bookmarkService)
    {
        _workflowManager = workflowManager;
        _workflowStorage = workflowStorage;
        _dbContext = dbContext;
        _bookmarkService = bookmarkService;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //
        //await _workflowManager.StartProcessAsync<TestProcess1>();
        //await Task.Delay(2000);
        await _workflowManager.StartProcessAsync<ParallelGatewayExample>();
        await Task.Delay(2000);
        //await _workflowManager.StartProcessAsync<TestProcess3>();
        //await Task.Delay(2000);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(10000);
            //await _workflowManager.StartProcessAsync<TestProcess2>();
            //var bookmark = _dbContext.WorkflowBookmarks.FirstOrDefault();
            //await _bookmarkService.BookmarkCompleteAsync(bookmark);
            //await Task.Delay(500000);
        }
    }
}