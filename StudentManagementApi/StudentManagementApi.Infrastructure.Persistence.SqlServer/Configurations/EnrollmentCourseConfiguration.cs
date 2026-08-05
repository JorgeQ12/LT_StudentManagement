using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentManagementApi.Domain.Enrollments;

namespace StudentManagementApi.Infrastructure.Persistence.SqlServer.Configurations;

internal sealed class EnrollmentCourseConfiguration : IEntityTypeConfiguration<EnrollmentCourse>
{
    public void Configure(EntityTypeBuilder<EnrollmentCourse> builder)
    {
        builder.ToTable("EnrollmentCourses");
        builder.HasKey(enrollmentCourse => enrollmentCourse.Id);
        builder.Property(enrollmentCourse => enrollmentCourse.Id).HasConversion(PersistenceConverters.EnrollmentCourseId).ValueGeneratedNever();
        builder.Property(enrollmentCourse => enrollmentCourse.EnrollmentId).HasConversion(PersistenceConverters.EnrollmentId);
        builder.Property(enrollmentCourse => enrollmentCourse.CourseId).HasConversion(PersistenceConverters.CourseId);
        builder.HasIndex(enrollmentCourse => new { enrollmentCourse.EnrollmentId, enrollmentCourse.CourseId }).IsUnique();
        builder.HasOne(enrollmentCourse => enrollmentCourse.Course).WithMany()
            .HasForeignKey(enrollmentCourse => enrollmentCourse.CourseId).OnDelete(DeleteBehavior.Restrict);
    }
}
