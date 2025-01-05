using MassTransitExample;
using MassTransitExample.Examples;
using MassTransitExample.Services;
using Microsoft.Extensions.Hosting;
using WorkflowProcessor.Services;

public class Worker : BackgroundService
{
    readonly WorkflowSender _sender;
    readonly WorkflowExecutor _workflowManager;
    private readonly WorkflowStorage _workflowStorage;
    private readonly WorkflowContext _dbContext;
    private readonly WorkflowBookmarkService _bookmarkService;

    public Worker(WorkflowSender sender, WorkflowExecutor workflowManager, WorkflowStorage workflowStorage, WorkflowContext dbContext, WorkflowBookmarkService bookmarkService)
    {
        _sender = sender;
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
        //await _workflowManager.StartProcessAsync<TestProcess2>();
        //await Task.Delay(2000);
        //await _workflowManager.StartProcessAsync<TestProcess3>();
        //await Task.Delay(2000);
        await _workflowManager.StartProcessAsync<TestProcess4>();
        await Task.Delay(2000);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(10000);
            //await Task.Delay(2000);
            //await _workflowManager.StartProcessAsync<TestProcess2>();
            //var bookmark = _dbContext.WorkflowBookmarks.FirstOrDefault();
            //await _bookmarkService.BookmarkCompleteAsync(bookmark);
            //await Task.Delay(500000);
        }
    }
}