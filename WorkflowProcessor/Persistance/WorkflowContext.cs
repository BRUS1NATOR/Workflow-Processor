// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WorkflowProcessor.Core;

public class WorkflowContext : DbContext
{
    //public DbSet<Workflow> Workflows { get; set; }
    public DbSet<WorkflowExecutionPoint> WorkflowExecutionPoints { get; set; }
    public DbSet<WorkflowBookmark> WorkflowBookmarks { get; set; }
    public DbSet<WorkflowInstance> WorkflowInstances { get; set; }
    public DbSet<UserTask> UserTasks { get; set; }

    public WorkflowContext()
    {

    }
    public WorkflowContext(DbContextOptions options) : base(options)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLoggerFactory(LoggerFactory.Create(builder =>
        {
            builder.AddFilter(_ => false);
        }));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //var workflow = modelBuilder.Entity<Workflow>();
        //workflow.HasKey(x => new { x.Name, x.Version });
        //workflow.Property(x => x.Name).HasColumnName("name");
        //workflow.Property(x => x.Version).HasColumnName("version");

        var workflowInstance = modelBuilder.Entity<WorkflowInstance>();
        workflowInstance.HasKey(x => x.Id);
        workflowInstance.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
        workflowInstance.Property(x => x.Status).HasColumnName("status");
        workflowInstance.Property(x => x.JsonData).HasColumnName("data").HasColumnType("jsonb");
        workflowInstance.Ignore(x => x.Context);

        workflowInstance.OwnsOne(w => w.Workflow,
            o =>
            {
                o.Property(p => p.Name).HasColumnName("workflow_name");
                o.Property(p => p.Version).HasColumnName("workflow_version");
                o.Ignore(p => p.ContextData);
                o.Ignore(p => p.Scheme);
                //o.Ignore(x => x.Scheme);
            });

        var workflowExPoint = modelBuilder.Entity<WorkflowExecutionPoint>();
        workflowExPoint.HasKey(x => x.Id);
        workflowExPoint.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
        workflowExPoint.Property(x => x.Status).HasColumnName("status");
        workflowExPoint.Property(x => x.StepId).HasColumnName("step_id");
        workflowExPoint.Property(x => x.ActivityType).HasColumnName("activity_type");

        workflowExPoint.Property(x => x.WorkflowInstanceId).HasColumnName("workflow_instance_id");
        workflowExPoint
            .HasOne(x => x.WorkflowInstance)
            .WithMany(x => x.WorkflowExecutionPoints)
            .HasForeignKey(x => x.WorkflowInstanceId);

        workflowExPoint
            .HasOne(x => x.WorkflowBookmark)
            .WithOne(x => x.WorkflowExecutionPoint)
            .HasForeignKey<WorkflowBookmark>(e => e.WorkflowExecutionPointId)
        .IsRequired();
    

    var workflowBookmark = modelBuilder.Entity<WorkflowBookmark>();
        workflowBookmark.HasKey(x => x.Id);
        workflowBookmark.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
        workflowBookmark.Property(x => x.Name).HasColumnName("name");
        workflowBookmark.Property(x => x.Status).HasColumnName("status");
        workflowBookmark.Property(x => x.WorkflowExecutionPointId).HasColumnName("workflow_execution_point_id");

        var userTask = modelBuilder.Entity<UserTask>();
        userTask.HasKey(x => x.Id);
        userTask.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
        userTask.Property(x => x.DisplayName).HasColumnName("display_name");
        userTask.Property(x => x.UserId).HasColumnName("user_id");


        userTask.OwnsOne(w => w.Metadata,
            o =>
            {
                //o.Property(p => p.UserId).HasColumnName("user_id");
                //o.Ignore(x => x.Scheme);
            });

        userTask.Property(x => x.WorkflowBookmarkId).HasColumnName("workflow_bookmark_id");
        userTask
            .HasOne(x => x.WorkflowBookmark)
            .WithMany(x => x.UserTasks)
            .HasForeignKey(x => x.WorkflowBookmarkId);
    }


    //public class JsonDataValueGenerator : Microsoft.EntityFrameworkCore.ValueGeneration.ValueGenerator
    //{
    //    public override bool GeneratesTemporaryValues => false;

    //    protected override object NextValue(EntityEntry entry)
    //    {
    //        return ((WorkflowInstance)entry.Entity).GetJsonContextValue();
    //    }
    //}
}