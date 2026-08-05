using Microsoft.EntityFrameworkCore;

namespace StudentManagementApi.Infrastructure.Persistence.SqlServer.Context;

internal sealed class StudentManagementReadDbContext(DbContextOptions<StudentManagementReadDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(StudentManagementDbContext).Assembly);
}
