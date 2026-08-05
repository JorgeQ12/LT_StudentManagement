using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentManagementApi.Domain.Accounts;
using StudentManagementApi.Domain.ValueObjects;

namespace StudentManagementApi.Infrastructure.Persistence.SqlServer.Configurations;

internal sealed class UserAccountConfiguration : IEntityTypeConfiguration<UserAccount>
{
    public void Configure(EntityTypeBuilder<UserAccount> builder)
    {
        builder.ToTable("UserAccounts");
        builder.HasKey(account => account.Id);
        builder.Property(account => account.Id).HasConversion(PersistenceConverters.UserAccountId).ValueGeneratedNever();
        builder.Property(account => account.Email).HasConversion(PersistenceConverters.Email).HasMaxLength(Email.MaximumLength).IsRequired();
        builder.Property(account => account.PasswordHash).HasMaxLength(500).IsRequired();
        builder.Property(account => account.Role).HasConversion<string>().HasMaxLength(32).IsRequired();
        builder.Property(account => account.Status).HasConversion<string>().HasMaxLength(32).IsRequired();
        builder.Property(account => account.StudentId).HasConversion(PersistenceConverters.NullableStudentId);
        builder.Property(account => account.RowVersion).IsRowVersion();
        builder.HasIndex(account => account.Email).IsUnique();
        builder.HasIndex(account => account.StudentId).IsUnique().HasFilter("[StudentId] IS NOT NULL");
    }
}
