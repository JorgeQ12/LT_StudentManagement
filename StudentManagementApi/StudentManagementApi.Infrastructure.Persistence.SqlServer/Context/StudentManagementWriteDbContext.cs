using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentManagementApi.Domain.Accounts;
using StudentManagementApi.Domain.Courses;
using StudentManagementApi.Domain.Enrollments;
using StudentManagementApi.Domain.Programs;
using StudentManagementApi.Domain.Professors;
using StudentManagementApi.Domain.Students;

namespace StudentManagementApi.Infrastructure.Persistence.SqlServer.Context;

public sealed class StudentManagementWriteDbContext(DbContextOptions<StudentManagementWriteDbContext> options)
    : DbContext(options), IDataProtectionKeyContext
{
    public DbSet<UserAccount> UserAccounts => Set<UserAccount>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<AcademicProgram> AcademicPrograms => Set<AcademicProgram>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Professor> Professors => Set<Professor>();
    public DbSet<TeachingAssignment> TeachingAssignments => Set<TeachingAssignment>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public DbSet<EnrollmentCourse> EnrollmentCourses => Set<EnrollmentCourse>();

    // Llavero de ASP.NET Core Data Protection compartido por todas las instancias de la
    // Lambda. Sin esto cada instancia genera sus propias llaves y el token antiforgery se
    // invalida en cada cold start o al balancear entre instancias.
    public DbSet<DataProtectionKey> DataProtectionKeys => Set<DataProtectionKey>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(StudentManagementWriteDbContext).Assembly);
}
