using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentManagementApi.Domain.Courses;
using StudentManagementApi.Domain.ValueObjects;

namespace StudentManagementApi.Infrastructure.Persistence.SqlServer.Configurations;

internal sealed class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.ToTable("Courses", table => table.HasCheckConstraint("CK_Courses_Credits", "[Credits] = 3"));
        builder.HasKey(course => course.Id);
        builder.Property(course => course.Id).HasConversion(PersistenceConverters.CourseId).ValueGeneratedNever();
        builder.Property(course => course.AcademicProgramId).HasConversion(PersistenceConverters.AcademicProgramId);
        builder.Property(course => course.Code).HasConversion(PersistenceConverters.CourseCode).HasMaxLength(CourseCode.MaximumLength).IsRequired();
        builder.Property(course => course.Name).HasMaxLength(Course.MaximumNameLength).IsRequired();
        builder.Property(course => course.Credits).IsRequired();
        builder.Property(course => course.Status).HasConversion<string>().HasMaxLength(32).IsRequired();
        builder.Property(course => course.RowVersion).IsRowVersion();
        builder.HasIndex(course => new { course.AcademicProgramId, course.Code }).IsUnique();
        builder.HasOne(course => course.AcademicProgram).WithMany().HasForeignKey(course => course.AcademicProgramId).OnDelete(DeleteBehavior.Restrict);
    }
}
