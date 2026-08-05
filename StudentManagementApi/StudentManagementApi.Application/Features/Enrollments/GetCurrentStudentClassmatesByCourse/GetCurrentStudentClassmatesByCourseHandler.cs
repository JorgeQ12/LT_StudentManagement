using Ardalis.Result;
using MediatR;
using StudentManagementApi.Application.Common.Errors;
using StudentManagementApi.Application.Common.Persistence;
using StudentManagementApi.Application.Common.Security;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Application.Specifications;
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
        var enrollment = await enrollmentRepository.FirstOrDefaultAsync(new ActiveEnrollmentByStudentSpec(studentId), cancellationToken);

        if (enrollment is null)
        {
            return ApplicationResults.NotFound<IReadOnlyCollection<ClassmatesByCourseResponse>>(ErrorCode.EnrollmentNotFound);
        }

        var courseIds = enrollment.Courses.Select(item => item.CourseId).ToArray();
        var classmates = await enrollmentRepository.ListAsync(new ActiveEnrollmentsByCourseIdsSpec(courseIds, studentId), cancellationToken);

        var response = enrollment
            .Courses.OrderBy(item => item.Course.Code.Value)
            .Select(item => new ClassmatesByCourseResponse(
                item.CourseId.Value,
                item.Course.Name,
                classmates
                    .Where(other => other.Courses.Any(course => course.CourseId == item.CourseId))
                    .Select(other => new ClassmateResponse(other.Student.FullName))
                    .OrderBy(classmate => classmate.FullName)
                    .ToArray()
            ))
            .ToArray();

        return Result<IReadOnlyCollection<ClassmatesByCourseResponse>>.Success(response);
    }
}
