using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StudentManagementApi.Application.Common.Persistence;
using StudentManagementApi.Application.Common.Security;
using StudentManagementApi.Infrastructure.Persistence.SqlServer.Bootstrap;
using StudentManagementApi.Infrastructure.Persistence.SqlServer.Context;
using StudentManagementApi.Infrastructure.Persistence.SqlServer.Repositories;
using StudentManagementApi.Infrastructure.Persistence.SqlServer.Security;
using StudentManagementApi.Infrastructure.Persistence.SqlServer.Transactions;

namespace StudentManagementApi.Infrastructure.Persistence.SqlServer;

public static class DependencyInjection
{
    public static IServiceCollection AddSqlServerPersistence(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<StudentManagementWriteDbContext>(options => options.UseSqlServer(connectionString, sql =>
            sql.MigrationsAssembly(typeof(StudentManagementWriteDbContext).Assembly.FullName).EnableRetryOnFailure()));
        services.AddDbContext<StudentManagementReadDbContext>(options => options.UseSqlServer(connectionString, sql => sql.EnableRetryOnFailure())
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
        services.AddScoped(typeof(IWriteRepository<>), typeof(EfWriteRepository<>));
        services.AddScoped(typeof(IReadRepository<>), typeof(EfReadRepository<>));
        services.AddScoped<ITransactionManager, EfTransactionManager>();
        services.AddScoped<IAccountSessionValidator, AccountSessionValidator>();
        services.AddScoped<AdministratorBootstrapper>();

        // Persiste el llavero de Data Protection en SQL Server para que el token antiforgery
        // sobreviva a los cold starts y se valide igual en cualquier instancia de la Lambda.
        services.AddDataProtection()
            .SetApplicationName("StudentManagementApi")
            .PersistKeysToDbContext<StudentManagementWriteDbContext>();

        return services;
    }
}
