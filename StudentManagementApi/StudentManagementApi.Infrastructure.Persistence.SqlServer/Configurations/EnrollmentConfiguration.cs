using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentManagementApi.Domain.Enrollments;

namespace StudentManagementApi.Infrastructure.Persistence.SqlServer.Configurations;

internal sealed class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
{
    public void Configure(EntityTypeBuilder<Enrollment> builder)
    {
        builder.ToTable("Enrollments", table => table.HasCheckConstraint("CK_Enrollments_Status", "[Status] IN ('Active', 'Cancelled')"));
        builder.HasKey(enrollment => enrollment.Id);
        builder.Property(enrollment => enrollment.Id).HasConversion(PersistenceConverters.EnrollmentId).ValueGeneratedNever();
        builder.Property(enrollment => enrollment.StudentId).HasConversion(PersistenceConverters.StudentId);
        builder.Property(enrollment => enrollment.AcademicProgramId).HasConversion(PersistenceConverters.AcademicProgramId);
        builder.Property(enrollment => enrollment.Status).HasConversion<string>().HasMaxLength(32).IsRequired();
        builder.Ignore(enrollment => enrollment.TotalCredits);
        builder.Property(enrollment => enrollment.RowVersion).IsRowVersion();
        builder.HasIndex(enrollment => enrollment.StudentId).IsUnique().HasFilter("[Status] = 'Active'");
        builder.HasOne(enrollment => enrollment.Student).WithMany(student => student.Enrollments)
            .HasForeignKey(enrollment => enrollment.StudentId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(enrollment => enrollment.AcademicProgram).WithMany()
            .HasForeignKey(enrollment => enrollment.AcademicProgramId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(enrollment => enrollment.Courses).WithOne(course => course.Enrollment)
            .HasForeignKey(course => course.EnrollmentId).OnDelete(DeleteBehavior.Cascade);
        builder.Navigation(enrollment => enrollment.Courses).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
