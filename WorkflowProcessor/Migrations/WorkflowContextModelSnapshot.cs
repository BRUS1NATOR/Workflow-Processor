﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WorkflowProcessor.Migrations
{
    [DbContext(typeof(WorkflowContext))]
    partial class WorkflowContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("UserTask", b =>
                {
                    b.Property<long>("UserId")
                        .HasColumnType("bigint")
                        .HasColumnName("user_id")
                        .HasAnnotation("Relational:JsonPropertyName", "userId");

                    b.Property<long>("WorkflowBookmarkId")
                        .HasColumnType("bigint")
                        .HasColumnName("workflow_bookmark_id")
                        .HasAnnotation("Relational:JsonPropertyName", "workflowBookmarkId");

                    b.HasKey("UserId", "WorkflowBookmarkId");

                    b.HasIndex("WorkflowBookmarkId");

                    b.ToTable("UserTasks");

                    b.HasAnnotation("Relational:JsonPropertyName", "userTasks");
                });

            modelBuilder.Entity("WorkflowBookmark", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name")
                        .HasAnnotation("Relational:JsonPropertyName", "name");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status")
                        .HasAnnotation("Relational:JsonPropertyName", "status");

                    b.Property<int>("Type")
                        .HasColumnType("integer")
                        .HasColumnName("type")
                        .HasAnnotation("Relational:JsonPropertyName", "type");

                    b.Property<long?>("WorkflowChildId")
                        .HasColumnType("bigint")
                        .HasColumnName("workflow_child_id")
                        .HasAnnotation("Relational:JsonPropertyName", "workflowChildId");

                    b.Property<long>("WorkflowExecutionPointId")
                        .HasColumnType("bigint")
                        .HasColumnName("workflow_execution_point_id")
                        .HasAnnotation("Relational:JsonPropertyName", "workflowExecutionPointId");

                    b.HasKey("Id");

                    b.HasIndex("WorkflowChildId")
                        .IsUnique();

                    b.HasIndex("WorkflowExecutionPointId")
                        .IsUnique();

                    b.ToTable("WorkflowBookmarks");
                });

            modelBuilder.Entity("WorkflowExecutionPoint", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string[]>("ActivatedStepsId")
                        .IsRequired()
                        .HasColumnType("text[]")
                        .HasColumnName("activated_steps_ids")
                        .HasAnnotation("Relational:JsonPropertyName", "activatedStepsId");

                    b.Property<string>("ActivityTypeName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("activity_type")
                        .HasAnnotation("Relational:JsonPropertyName", "activityType");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status")
                        .HasAnnotation("Relational:JsonPropertyName", "status");

                    b.Property<string>("StepId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("step_id")
                        .HasAnnotation("Relational:JsonPropertyName", "stepId");

                    b.Property<long>("WorkflowInstanceId")
                        .HasColumnType("bigint")
                        .HasColumnName("workflow_instance_id")
                        .HasAnnotation("Relational:JsonPropertyName", "workflowInstanceId");

                    b.HasKey("Id");

                    b.HasIndex("WorkflowInstanceId");

                    b.ToTable("WorkflowExecutionPoints");

                    b.HasAnnotation("Relational:JsonPropertyName", "workflowExecutionPoint");
                });

            modelBuilder.Entity("WorkflowProcessor.Core.WorkflowInstance", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long?>("Initiator")
                        .HasColumnType("bigint")
                        .HasColumnName("initiator")
                        .HasAnnotation("Relational:JsonPropertyName", "initiator");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name")
                        .HasAnnotation("Relational:JsonPropertyName", "name");

                    b.Property<long?>("ParentId")
                        .HasColumnType("bigint")
                        .HasColumnName("parent_id")
                        .HasAnnotation("Relational:JsonPropertyName", "parent_id");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status")
                        .HasAnnotation("Relational:JsonPropertyName", "status");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("WorkflowInstances");

                    b.HasAnnotation("Relational:JsonPropertyName", "workflowInstance");
                });

            modelBuilder.Entity("UserTask", b =>
                {
                    b.HasOne("WorkflowBookmark", "WorkflowBookmark")
                        .WithMany("UserTasks")
                        .HasForeignKey("WorkflowBookmarkId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WorkflowBookmark");
                });

            modelBuilder.Entity("WorkflowBookmark", b =>
                {
                    b.HasOne("WorkflowProcessor.Core.WorkflowInstance", "WorkflowChild")
                        .WithOne()
                        .HasForeignKey("WorkflowBookmark", "WorkflowChildId");

                    b.HasOne("WorkflowExecutionPoint", "WorkflowExecutionPoint")
                        .WithOne("WorkflowBookmark")
                        .HasForeignKey("WorkflowBookmark", "WorkflowExecutionPointId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WorkflowChild");

                    b.Navigation("WorkflowExecutionPoint");
                });

            modelBuilder.Entity("WorkflowExecutionPoint", b =>
                {
                    b.HasOne("WorkflowProcessor.Core.WorkflowInstance", "WorkflowInstance")
                        .WithMany("WorkflowExecutionPoints")
                        .HasForeignKey("WorkflowInstanceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WorkflowInstance");
                });

            modelBuilder.Entity("WorkflowProcessor.Core.WorkflowInstance", b =>
                {
                    b.HasOne("WorkflowProcessor.Core.WorkflowInstance", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId")
                        .HasConstraintName("parent_children_fk");

                    b.OwnsOne("WorkflowProcessor.Core.WorkflowInfo", "WorkflowInfo", b1 =>
                        {
                            b1.Property<long>("WorkflowInstanceId")
                                .HasColumnType("bigint");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("workflow_name")
                                .HasAnnotation("Relational:JsonPropertyName", "name");

                            b1.Property<int>("Version")
                                .HasColumnType("integer")
                                .HasColumnName("workflow_version")
                                .HasAnnotation("Relational:JsonPropertyName", "version");

                            b1.HasKey("WorkflowInstanceId");

                            b1.ToTable("WorkflowInstances");

                            b1.HasAnnotation("Relational:JsonPropertyName", "workflowInfo");

                            b1.WithOwner()
                                .HasForeignKey("WorkflowInstanceId");
                        });

                    b.OwnsOne("WorkflowProcessor.Persistance.Context.Context", "Context", b1 =>
                        {
                            b1.Property<long>("WorkflowInstanceId")
                                .HasColumnType("bigint");

                            b1.Property<string>("JsonData")
                                .IsRequired()
                                .HasColumnType("jsonb")
                                .HasColumnName("data");

                            b1.HasKey("WorkflowInstanceId");

                            b1.ToTable("WorkflowInstances");

                            b1.HasAnnotation("Relational:JsonPropertyName", "context");

                            b1.WithOwner("WorkflowInstance")
                                .HasForeignKey("WorkflowInstanceId");

                            b1.Navigation("WorkflowInstance");
                        });

                    b.Navigation("Context")
                        .IsRequired();

                    b.Navigation("Parent");

                    b.Navigation("WorkflowInfo")
                        .IsRequired();
                });

            modelBuilder.Entity("WorkflowBookmark", b =>
                {
                    b.Navigation("UserTasks");
                });

            modelBuilder.Entity("WorkflowExecutionPoint", b =>
                {
                    b.Navigation("WorkflowBookmark");
                });

            modelBuilder.Entity("WorkflowProcessor.Core.WorkflowInstance", b =>
                {
                    b.Navigation("Children");

                    b.Navigation("WorkflowExecutionPoints");
                });
#pragma warning restore 612, 618
        }
    }
}
