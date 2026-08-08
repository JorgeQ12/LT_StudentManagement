using Ardalis.Specification;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Domain;
using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Enrollments;

namespace StudentManagementApi.Application.Specifications;

internal sealed class EnrollmentResponseByStudentSpec
    : SingleResultSpecification<Enrollment, EnrollmentResponse>
{
    public EnrollmentResponseByStudentSpec(StudentId studentId) =>
        Query.Where(enrollment =>
                enrollment.StudentId == studentId && enrollment.Status == EnrollmentStatus.Active)
            .AsNoTracking()
            .Select(enrollment => new EnrollmentResponse(
                enrollment.Id.Value,
                enrollment.StudentId.Value,
                new AcademicProgramResponse(
                    enrollment.AcademicProgram.Id.Value,
                    enrollment.AcademicProgram.Code.Value,
                    enrollment.AcademicProgram.Name,
                    enrollment.AcademicProgram.Description,
                    enrollment.AcademicProgram.Status),
                enrollment.Status,
                enrollment.Courses.Count * global::StudentManagementApi.Domain.Courses.Course.RequiredCredits,
                enrollment.Courses
                    .OrderBy(item => item.Course.Code)
                    .Select(item => new EnrollmentCourseResponse(
                        item.Course.Id.Value,
                        item.Course.Code.Value,
                        item.Course.Name,
                        item.Course.Credits,
                        item.Course.TeachingAssignment!.ProfessorId.Value,
                        item.Course.TeachingAssignment.Professor.FirstName + " " +
                        item.Course.TeachingAssignment.Professor.LastName))
                    .ToArray(),
                enrollment.CreatedAtUtc,
                enrollment.CancelledAtUtc));
}
