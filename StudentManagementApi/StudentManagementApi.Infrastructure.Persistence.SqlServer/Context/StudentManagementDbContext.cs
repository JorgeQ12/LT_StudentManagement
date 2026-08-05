using Microsoft.EntityFrameworkCore;
using StudentManagementApi.Domain.Accounts;
using StudentManagementApi.Domain.Courses;
using StudentManagementApi.Domain.Enrollments;
using StudentManagementApi.Domain.Programs;
using StudentManagementApi.Domain.Professors;
using StudentManagementApi.Domain.Students;

namespace StudentManagementApi.Infrastructure.Persistence.SqlServer.Context;

public sealed class StudentManagementDbContext(DbContextOptions<StudentManagementDbContext> options) : DbContext(options)
{
    public DbSet<UserAccount> UserAccounts => Set<UserAccount>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<AcademicProgram> AcademicPrograms => Set<AcademicProgram>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Professor> Professors => Set<Professor>();
    public DbSet<TeachingAssignment> TeachingAssignments => Set<TeachingAssignment>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public DbSet<EnrollmentCourse> EnrollmentCourses => Set<EnrollmentCourse>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(StudentManagementDbContext).Assembly);
}
