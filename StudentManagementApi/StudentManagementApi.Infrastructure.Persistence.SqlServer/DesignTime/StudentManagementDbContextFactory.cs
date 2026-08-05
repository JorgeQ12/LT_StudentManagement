using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using StudentManagementApi.Infrastructure.Persistence.SqlServer.Context;

namespace StudentManagementApi.Infrastructure.Persistence.SqlServer.DesignTime;

internal sealed class StudentManagementDbContextFactory : IDesignTimeDbContextFactory<StudentManagementDbContext>
{
    public StudentManagementDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__StudentManagementDb");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            Env.NoClobber().TraversePath().Load();
            connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__StudentManagementDb");
        }

        if (string.IsNullOrWhiteSpace(connectionString)) throw new InvalidOperationException(PersistenceConstants.ConnectionStringName);
        var options = new DbContextOptionsBuilder<StudentManagementDbContext>().UseSqlServer(connectionString).Options;
        return new StudentManagementDbContext(options);
    }
}
