using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StudentManagementApi.Domain.Abstractions;
using CourseCodeValue = StudentManagementApi.Domain.ValueObjects.CourseCode;
using DocumentNumberValue = StudentManagementApi.Domain.ValueObjects.DocumentNumber;
using EmailValue = StudentManagementApi.Domain.ValueObjects.Email;
using PhoneNumberValue = StudentManagementApi.Domain.ValueObjects.PhoneNumber;
using ProgramCodeValue = StudentManagementApi.Domain.ValueObjects.ProgramCode;

namespace StudentManagementApi.Infrastructure.Persistence.SqlServer.Configurations;

internal static class PersistenceConverters
{
    public static readonly ValueConverter<UserAccountId, Guid> UserAccountId = new(id => id.Value, value => new(value));
    public static readonly ValueConverter<StudentId, Guid> StudentId = new(id => id.Value, value => new(value));
    public static readonly ValueConverter<StudentId?, Guid?> NullableStudentId = new(
        id => id.HasValue ? id.Value.Value : null,
        value => value.HasValue ? new StudentId(value.Value) : null);
    public static readonly ValueConverter<AcademicProgramId, Guid> AcademicProgramId = new(id => id.Value, value => new(value));
    public static readonly ValueConverter<CourseId, Guid> CourseId = new(id => id.Value, value => new(value));
    public static readonly ValueConverter<ProfessorId, Guid> ProfessorId = new(id => id.Value, value => new(value));
    public static readonly ValueConverter<TeachingAssignmentId, Guid> TeachingAssignmentId = new(id => id.Value, value => new(value));
    public static readonly ValueConverter<EnrollmentId, Guid> EnrollmentId = new(id => id.Value, value => new(value));
    public static readonly ValueConverter<EnrollmentCourseId, Guid> EnrollmentCourseId = new(id => id.Value, value => new(value));
    public static readonly ValueConverter<EmailValue, string> Email = new(value => value.Value, value => EmailValue.Create(value));
    public static readonly ValueConverter<DocumentNumberValue, string> DocumentNumber = new(value => value.Value, value => DocumentNumberValue.Create(value));
    public static readonly ValueConverter<PhoneNumberValue, string> PhoneNumber = new(value => value.Value, value => PhoneNumberValue.Create(value));
    public static readonly ValueConverter<ProgramCodeValue, string> ProgramCode = new(value => value.Value, value => ProgramCodeValue.Create(value));
    public static readonly ValueConverter<CourseCodeValue, string> CourseCode = new(value => value.Value, value => CourseCodeValue.Create(value));
}
