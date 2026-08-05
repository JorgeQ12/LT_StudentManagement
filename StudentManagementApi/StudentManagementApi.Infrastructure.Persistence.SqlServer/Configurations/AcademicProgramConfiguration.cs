using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentManagementApi.Domain.Programs;
using StudentManagementApi.Domain.ValueObjects;

namespace StudentManagementApi.Infrastructure.Persistence.SqlServer.Configurations;

internal sealed class AcademicProgramConfiguration : IEntityTypeConfiguration<AcademicProgram>
{
    public void Configure(EntityTypeBuilder<AcademicProgram> builder)
    {
        builder.ToTable("AcademicPrograms");
        builder.HasKey(program => program.Id);
        builder.Property(program => program.Id).HasConversion(PersistenceConverters.AcademicProgramId).ValueGeneratedNever();
        builder.Property(program => program.Code).HasConversion(PersistenceConverters.ProgramCode).HasMaxLength(ProgramCode.MaximumLength).IsRequired();
        builder.Property(program => program.Name).HasMaxLength(AcademicProgram.MaximumNameLength).IsRequired();
        builder.Property(program => program.Description).HasMaxLength(AcademicProgram.MaximumDescriptionLength).IsRequired();
        builder.Property(program => program.Status).HasConversion<string>().HasMaxLength(32).IsRequired();
        builder.Property(program => program.RowVersion).IsRowVersion();
        builder.HasIndex(program => program.Code).IsUnique();
    }
}
