using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentManagementApi.Domain.Students;
using StudentManagementApi.Domain.ValueObjects;

namespace StudentManagementApi.Infrastructure.Persistence.SqlServer.Configurations;

internal sealed class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("Students");
        builder.HasKey(student => student.Id);
        builder.Property(student => student.Id).HasConversion(PersistenceConverters.StudentId).ValueGeneratedNever();
        builder.Property(student => student.AccountId).HasConversion(PersistenceConverters.UserAccountId);
        builder.Property(student => student.FirstName).HasMaxLength(Student.MaximumNameLength).IsRequired();
        builder.Property(student => student.LastName).HasMaxLength(Student.MaximumNameLength).IsRequired();
        builder.Property(student => student.DocumentNumber).HasConversion(PersistenceConverters.DocumentNumber)
            .HasMaxLength(DocumentNumber.MaximumLength).IsRequired();
        builder.Property(student => student.PhoneNumber).HasConversion(PersistenceConverters.PhoneNumber)
            .HasMaxLength(PhoneNumber.MaximumLength).IsRequired();
        builder.Property(student => student.DateOfBirth).HasColumnType("date");
        builder.Property(student => student.Status).HasConversion<string>().HasMaxLength(32).IsRequired();
        builder.Property(student => student.RowVersion).IsRowVersion();
        builder.HasIndex(student => student.AccountId).IsUnique();
        builder.HasIndex(student => student.DocumentNumber).IsUnique();
        builder.HasOne(student => student.Account).WithOne().HasForeignKey<Student>(student => student.AccountId).OnDelete(DeleteBehavior.Restrict);
    }
}
