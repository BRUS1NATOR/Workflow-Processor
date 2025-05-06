using MassTransit;
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
    x.AddWorkflowConsumers();
    x.UsingInMemory((context, cfg) =>
    {
        //var connectionString = new Uri("RabbitMQ_URL");
        //cfg.Host(connectionString);

        cfg.ConfigureEndpoints(context);

    });
});
builder.Services.AddTransient<IWorkflowUserService, WorkflowUserService>();
builder.Services.AddWorkflowServices();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();
builder.Services.AddOptions();


//string CORS_POLICY = "SPA";
//string CORS_URL = "http://localhost:5173";

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy(name: CORS_POLICY,
//        policy =>
//        {
//            policy.WithOrigins(CORS_URL).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
//        });
//});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<WorkflowContext>();
    if (db.Database.GetPendingMigrations().Any())
    {
        await db.Database.MigrateAsync();
    }
}
//app.AddExampleWorkflows();
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

//app.UseCors(CORS_POLICY);
app.AddExampleWorkflows();
app.UseAuthorization();

app.MapControllers();

app.Run();
