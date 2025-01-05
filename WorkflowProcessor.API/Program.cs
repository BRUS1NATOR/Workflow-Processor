using MassTransit;
using MassTransitExample.MasstransitWorkflow;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using WorkflowProcessor.Extensions;
using WorkflowProcessor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging(config =>
{
    config.AddDebug();
    config.AddConsole();
});

// Add services to the container.

builder.Services.AddDbContext<WorkflowContext>(x => x.UseNpgsql(@"Server=127.0.0.1;Port=5432;Database=myworkflow;User Id=postgres;Password=root;"));
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<WorkflowStartConsumer>();
    x.AddConsumer<WorkflowExecuteStepConsumer>();
    x.UsingInMemory((context, cfg) =>
    {
        //var connectionString = new Uri("RabbitMQ_URL");
        //cfg.Host(connectionString);

        cfg.ConfigureEndpoints(context);

    });
});

builder.Services.AddWorkflow();
builder.Services.AddTransient<WorkflowInstanceFactory>();
builder.Services.AddTransient<WorkflowBookmarkService>();
//builder.Services.AddHostedService<Worker>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();
builder.Services.AddOptions();


var app = builder.Build();
app.AddWorkflows();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(x =>
    {
        x.RouteTemplate = "openapi/{documentName}.json";
    });
    app.MapScalarApiReference();
}
//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
