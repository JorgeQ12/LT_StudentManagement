using Ardalis.Result;
using MediatR;
using StudentManagementApi.Application.Common.Errors;
using StudentManagementApi.Application.Common.Persistence;
using StudentManagementApi.Application.Common.Security;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Application.Specifications;
using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Enrollments;

namespace StudentManagementApi.Application.Features.Enrollments.GetCurrentStudentClassmatesByCourse;

internal sealed class GetCurrentStudentClassmatesByCourseHandler(ICurrentUser currentUser, IReadRepository<Enrollment> enrollmentRepository)
    : IRequestHandler<GetCurrentStudentClassmatesByCourseQuery, Result<IReadOnlyCollection<ClassmatesByCourseResponse>>>
{
    public async Task<Result<IReadOnlyCollection<ClassmatesByCourseResponse>>> Handle(
        GetCurrentStudentClassmatesByCourseQuery request,
        CancellationToken cancellationToken
    )
    {
        if (currentUser.StudentId is null)
        {
            return ApplicationResults.Forbidden<IReadOnlyCollection<ClassmatesByCourseResponse>>();
        }

        var studentId = currentUser.StudentId.Value;
        var enrollment = await enrollmentRepository.FirstOrDefaultAsync(
            new ActiveEnrollmentCoursesByStudentSpec(studentId),
            cancellationToken);

        if (enrollment is null)
        {
            return ApplicationResults.NotFound<IReadOnlyCollection<ClassmatesByCourseResponse>>(ErrorCode.EnrollmentNotFound);
        }

        var courseIds = enrollment.Courses.Select(course => new CourseId(course.CourseId)).ToArray();
        var classmates = await enrollmentRepository.ListAsync(
            new ActiveClassmateEnrollmentsByCourseIdsSpec(courseIds, studentId),
            cancellationToken);

        var response = enrollment
            .Courses
            .Select(course => new ClassmatesByCourseResponse(
                course.CourseId,
                course.CourseName,
                classmates
                    .Where(classmate => classmate.CourseIds.Contains(course.CourseId))
                    .Select(classmate => new ClassmateResponse(classmate.FullName))
                    .OrderBy(classmate => classmate.FullName)
                    .ToArray()
            ))
            .ToArray();

        return Result<IReadOnlyCollection<ClassmatesByCourseResponse>>.Success(response);
    }
}
