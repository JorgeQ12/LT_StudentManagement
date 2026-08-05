using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Domain.Accounts;
using StudentManagementApi.Domain.Courses;
using StudentManagementApi.Domain.Enrollments;
using StudentManagementApi.Domain.Programs;
using StudentManagementApi.Domain.Professors;
using StudentManagementApi.Domain.Students;

namespace StudentManagementApi.Application.Common.Mappings;

internal static class ResponseMappings
{
    public static StudentResponse ToResponse(this Student student) => new(
        student.Id.Value,
        student.FirstName,
        student.LastName,
        student.DocumentNumber.Value,
        student.DateOfBirth,
        student.PhoneNumber.Value,
        student.Account.Email.Value,
        student.Status);

    public static StudentResponse ToResponse(this Student student, string email) => new(student.Id.Value, student.FirstName, student.LastName,
        student.DocumentNumber.Value, student.DateOfBirth, student.PhoneNumber.Value, email, student.Status);

    public static AuthenticatedUserResponse ToAuthenticatedUser(this UserAccount account) => new(
        account.Id.Value,
        account.StudentId?.Value,
        account.Email.Value,
        account.Role);

    public static AcademicProgramResponse ToResponse(this AcademicProgram program) => new(
        program.Id.Value,
        program.Code.Value,
        program.Name,
        program.Description,
        program.Status);

    public static CourseResponse ToResponse(this Course course) => new(
        course.Id.Value,
        course.AcademicProgramId.Value,
        course.Code.Value,
        course.Name,
        course.Credits,
        course.Status,
        course.TeachingAssignment?.ProfessorId.Value,
        course.TeachingAssignment?.Professor.FullName);

    public static ProfessorResponse ToResponse(this Professor professor) => new(
        professor.Id.Value,
        professor.FirstName,
        professor.LastName,
        professor.Status,
        professor.TeachingAssignments.Count);

    public static EnrollmentResponse ToResponse(this Enrollment enrollment) => new(
        enrollment.Id.Value,
        enrollment.StudentId.Value,
        enrollment.AcademicProgram.ToResponse(),
        enrollment.Status,
        enrollment.TotalCredits,
        enrollment.Courses.Select(item => new EnrollmentCourseResponse(
            item.Course.Id.Value,
            item.Course.Code.Value,
            item.Course.Name,
            item.Course.Credits,
            item.Course.TeachingAssignment!.ProfessorId.Value,
            item.Course.TeachingAssignment.Professor.FullName)).ToArray(),
        enrollment.CreatedAtUtc,
        enrollment.CancelledAtUtc);

    public static EnrollmentResponse ToResponse(
        this Enrollment enrollment,
        AcademicProgram academicProgram,
        IReadOnlyCollection<Course> courses) => new(
        enrollment.Id.Value,
        enrollment.StudentId.Value,
        academicProgram.ToResponse(),
        enrollment.Status,
        enrollment.TotalCredits,
        courses.Select(course => new EnrollmentCourseResponse(
            course.Id.Value,
            course.Code.Value,
            course.Name,
            course.Credits,
            course.TeachingAssignment!.ProfessorId.Value,
            course.TeachingAssignment.Professor.FullName)).ToArray(),
        enrollment.CreatedAtUtc,
        enrollment.CancelledAtUtc);
}
