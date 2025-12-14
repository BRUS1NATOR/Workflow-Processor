// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WorkflowProcessor.Console.Examples;
using WorkflowProcessor.Extensions;

var app = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddDbContext<IWorkflowDbContext, WorkflowDbContext>(x => x.UseNpgsql(@"Server=127.0.0.1;Port=5432;Database=myworkflow;User Id=postgres;Password=root;"));
        //services.AddInMemoryMassTransit();

        services.AddWorkflowInMemoryBus();
        services.AddWorkflowServices();
        services.AddHostedService<ProcessExampleWorker>();
    })
    .Build();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<WorkflowDbContext>();
    if (db.Database.GetPendingMigrations().Any())
    {
        await db.Database.MigrateAsync();
    }
}
app.AddWorkflow<TestProcess1>();
//
app.Run();