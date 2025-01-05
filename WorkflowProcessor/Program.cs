// See https://aka.ms/new-console-template for more information
using MassTransit;
using MassTransitExample.MasstransitWorkflow;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WorkflowProcessor.Extensions;
using WorkflowProcessor.Services;

var app = Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
        {
            services.AddDbContext<WorkflowContext>(x => x.UseNpgsql(@"Server=127.0.0.1;Port=5432;Database=myworkflow;User Id=postgres;Password=root;"));
            services.AddMassTransit(x =>
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

            services.AddWorkflow();
            services.AddTransient<WorkflowInstanceFactory>();
            services.AddTransient<WorkflowBookmarkService>();
            services.AddHostedService<Worker>();
        })
        .Build();
app.AddWorkflows();
app.Run();