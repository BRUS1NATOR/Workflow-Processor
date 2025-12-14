using Microsoft.Extensions.Hosting;
using WorkflowProcessor.Core;
using WorkflowProcessor.Services;

public class ProcessExampleWorker : BackgroundService
{
    readonly WorkflowExecutor _workflowManager;
    private readonly IWorkflowStorage _workflowStorage;
    private readonly WorkflowDbContext _dbContext;
    private readonly WorkflowBookmarkService _bookmarkService;

    public ProcessExampleWorker(WorkflowExecutor workflowManager, IWorkflowStorage workflowStorage, WorkflowDbContext dbContext, WorkflowBookmarkService bookmarkService)
    {
        _workflowManager = workflowManager;
        _workflowStorage = workflowStorage;
        _dbContext = dbContext;
        _bookmarkService = bookmarkService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        string? userInput = string.Empty;
        Workflow? workflow = null;
        Console.WriteLine("Please enter workflow name");
        while (string.IsNullOrEmpty(userInput) || workflow == null)
        {
            userInput = Console.ReadLine();
            if (string.IsNullOrEmpty(userInput))
            {
                Console.WriteLine("Please enter valid workflow name");
                continue;
            }
            workflow = _workflowStorage.GetWorkflow(userInput, null);
            if (workflow is null)
            {
                Console.WriteLine($"Workflow with name {userInput} not found");
                continue;
            }
            // Start process
            await _workflowManager.StartProcessAsync(workflow);
            // Infinite wait
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(int.MaxValue);
            }
        }
    }
}