using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trycore.Evm.Domain.Entities;

namespace Trycore.Evm.Infrastructure.Persistence.Configurations;

public class ProjectActivityConfiguration : IEntityTypeConfiguration<ProjectActivity>
{
    public void Configure(EntityTypeBuilder<ProjectActivity> builder)
    {
        builder.ToTable("activities");

        builder.HasKey(activity => activity.Id);

        builder.Property(activity => activity.Id)
            .HasColumnName("id");

        builder.Property(activity => activity.ProjectId)
            .HasColumnName("project_id")
            .IsRequired();

        builder.Property(activity => activity.Name)
            .HasColumnName("name")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(activity => activity.Bac)
            .HasColumnName("bac")
            .HasColumnType("numeric(18,2)")
            .IsRequired();

        builder.Property(activity => activity.PlannedProgressPercent)
            .HasColumnName("planned_progress_percent")
            .HasColumnType("numeric(5,2)")
            .IsRequired();

        builder.Property(activity => activity.ActualProgressPercent)
            .HasColumnName("actual_progress_percent")
            .HasColumnType("numeric(5,2)")
            .IsRequired();

        builder.Property(activity => activity.ActualCost)
            .HasColumnName("actual_cost")
            .HasColumnType("numeric(18,2)")
            .IsRequired();

        builder.Property(activity => activity.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(activity => activity.UpdatedAt)
            .HasColumnName("updated_at");

        builder.HasOne(activity => activity.Project)
            .WithMany(project => project.Activities)
            .HasForeignKey(activity => activity.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(activity => activity.ProjectId)
            .HasDatabaseName("ix_activities_project_id");

        builder.ToTable(table =>
        {
            table.HasCheckConstraint("ck_activities_bac", "bac >= 0");
            table.HasCheckConstraint("ck_activities_actual_cost", "actual_cost >= 0");
            table.HasCheckConstraint("ck_activities_planned_percent", "planned_progress_percent >= 0 AND planned_progress_percent <= 100");
            table.HasCheckConstraint("ck_activities_actual_percent", "actual_progress_percent >= 0 AND actual_progress_percent <= 100");
        });
    }
}