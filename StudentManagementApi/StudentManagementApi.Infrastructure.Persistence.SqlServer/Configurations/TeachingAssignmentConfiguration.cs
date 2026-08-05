using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentManagementApi.Domain.Professors;

namespace StudentManagementApi.Infrastructure.Persistence.SqlServer.Configurations;

internal sealed class TeachingAssignmentConfiguration : IEntityTypeConfiguration<TeachingAssignment>
{
    public void Configure(EntityTypeBuilder<TeachingAssignment> builder)
    {
        builder.ToTable("TeachingAssignments");
        builder.HasKey(assignment => assignment.Id);
        builder.Property(assignment => assignment.Id).HasConversion(PersistenceConverters.TeachingAssignmentId).ValueGeneratedNever();
        builder.Property(assignment => assignment.ProfessorId).HasConversion(PersistenceConverters.ProfessorId);
        builder.Property(assignment => assignment.CourseId).HasConversion(PersistenceConverters.CourseId);
        builder.Property(assignment => assignment.AcademicProgramId).HasConversion(PersistenceConverters.AcademicProgramId);
        builder.HasIndex(assignment => assignment.CourseId).IsUnique();
        builder.HasIndex(assignment => new { assignment.ProfessorId, assignment.CourseId }).IsUnique();
        builder.HasOne(assignment => assignment.Course).WithOne(course => course.TeachingAssignment)
            .HasForeignKey<TeachingAssignment>(assignment => assignment.CourseId).OnDelete(DeleteBehavior.Restrict);
    }
}
