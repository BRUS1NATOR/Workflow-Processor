// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WorkflowProcessor.Core;

public class WorkflowDbContext : DbContext, IWorkflowDbContext
{
    public DbSet<WorkflowExecutionPoint> WorkflowExecutionPoints { get; set; }
    public DbSet<WorkflowBookmark> WorkflowBookmarks { get; set; }
    public DbSet<UserTask> UserTasks { get; set; }
    public DbSet<WorkflowInstance> WorkflowInstances { get; set; }

    public WorkflowDbContext(DbContextOptions<WorkflowDbContext> options) : base(options)
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
        // Workflow Instance
        var workflowInstance = modelBuilder.Entity<WorkflowInstance>();
        workflowInstance.HasKey(x => x.Id);
        workflowInstance.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();

        workflowInstance
            .HasOne(x => x.Parent)
            .WithMany(x => x.Children)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("parent_children_fk");

        workflowInstance.Property(x => x.Name).HasColumnName("name");
        workflowInstance.Property(x => x.Status).HasColumnName("status");
        workflowInstance.Property(x => x.Initiator).HasColumnName("initiator");
        workflowInstance.Property(x => x.ParentId).HasColumnName("parent_id");
        // ContextData
        workflowInstance.OwnsOne(x => x.Context, e =>
        {
            e.WithOwner(x => x.WorkflowInstance);
            e.Property(x => x.JsonData).HasColumnName("data").HasColumnType("jsonb");
            e.Ignore(x => x.DataObject);
        });

        workflowInstance.OwnsOne(w => w.WorkflowInfo,
            o =>
            {
                o.Property(p => p.Name).HasColumnName("workflow_name");
                o.Property(p => p.Version).HasColumnName("workflow_version");
                o.Ignore(p => p.IsAllowedToRunFromWeb);
            });

        // Execution Point
        var workflowExecutionPoint = modelBuilder.Entity<WorkflowExecutionPoint>();
        workflowExecutionPoint.HasKey(x => x.Id);
        workflowExecutionPoint.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
        workflowExecutionPoint.Property(x => x.Status).HasColumnName("status");

        workflowExecutionPoint.Property(x => x.StepId).HasColumnName("step_id");
        workflowExecutionPoint.Property(x => x.ActivatedStepsId).HasColumnName("activated_steps_ids");
        workflowExecutionPoint.Property(x => x.ActivityTypeName).HasColumnName("activity_type");

        workflowExecutionPoint.Property(x => x.WorkflowInstanceId).HasColumnName("workflow_instance_id");
        workflowExecutionPoint
            .HasOne(x => x.WorkflowInstance)
            .WithMany(x => x.WorkflowExecutionPoints)
            .HasForeignKey(x => x.WorkflowInstanceId);

        workflowExecutionPoint
            .HasOne(x => x.WorkflowBookmark)
            .WithOne(x => x.WorkflowExecutionPoint)
            .HasForeignKey<WorkflowBookmark>(e => e.WorkflowExecutionPointId)
        .IsRequired();

        // Bookmark
        var workflowBookmark = modelBuilder.Entity<WorkflowBookmark>();
        workflowBookmark.HasKey(x => x.Id);
        workflowBookmark.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
        workflowBookmark.Property(x => x.Name).HasColumnName("name");
        workflowBookmark.Property(x => x.Status).HasColumnName("status");
        workflowBookmark.Property(x => x.Type).HasColumnName("type");
        workflowBookmark.Property(x => x.WorkflowChildId).HasColumnName("workflow_child_id");
        workflowBookmark.Property(x => x.WorkflowExecutionPointId).HasColumnName("workflow_execution_point_id");

        workflowBookmark
            .HasOne(x => x.WorkflowChild)
            .WithOne()
            .HasForeignKey<WorkflowBookmark>(e => e.WorkflowChildId);

        // UserTask
        var userTask = modelBuilder.Entity<UserTask>();
        userTask.HasKey(x => new { x.UserId, x.WorkflowBookmarkId });
        userTask.Property(x => x.UserId).HasColumnName("user_id");
        userTask.Property(x => x.WorkflowBookmarkId).HasColumnName("workflow_bookmark_id");

        userTask
            .HasOne(x => x.WorkflowBookmark)
            .WithMany(x => x.UserTasks)
            .HasForeignKey(e => e.WorkflowBookmarkId);
    }
}