using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentManagementApi.Domain.Professors;

namespace StudentManagementApi.Infrastructure.Persistence.SqlServer.Configurations;

internal sealed class ProfessorConfiguration : IEntityTypeConfiguration<Professor>
{
    public void Configure(EntityTypeBuilder<Professor> builder)
    {
        builder.ToTable("Professors");
        builder.HasKey(professor => professor.Id);
        builder.Property(professor => professor.Id).HasConversion(PersistenceConverters.ProfessorId).ValueGeneratedNever();
        builder.Property(professor => professor.FirstName).HasMaxLength(Professor.MaximumNameLength).IsRequired();
        builder.Property(professor => professor.LastName).HasMaxLength(Professor.MaximumNameLength).IsRequired();
        builder.Property(professor => professor.Status).HasConversion<string>().HasMaxLength(32).IsRequired();
        builder.Property(professor => professor.RowVersion).IsRowVersion();
        builder.HasMany(professor => professor.TeachingAssignments).WithOne(assignment => assignment.Professor)
            .HasForeignKey(assignment => assignment.ProfessorId).OnDelete(DeleteBehavior.Restrict);
        builder.Navigation(professor => professor.TeachingAssignments).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
