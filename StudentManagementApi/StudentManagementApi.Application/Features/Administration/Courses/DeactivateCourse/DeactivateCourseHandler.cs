using Ardalis.Result;
using MediatR;
using StudentManagementApi.Application.Common.Errors;
using StudentManagementApi.Application.Common.Messaging;
using StudentManagementApi.Application.Common.Persistence;
using StudentManagementApi.Application.Specifications;
using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Courses;
using StudentManagementApi.Domain.Enrollments;

namespace StudentManagementApi.Application.Features.Administration.Courses.DeactivateCourse;

internal sealed class DeactivateCourseHandler(IWriteRepository<Course> courseRepository, IWriteRepository<Enrollment> enrollmentRepository,
    TimeProvider timeProvider) : IRequestHandler<DeactivateCourseCommand, Result<NoContentResponse>>
{
    public async Task<Result<NoContentResponse>> Handle(DeactivateCourseCommand request, CancellationToken cancellationToken)
    {
        var courseId = new CourseId(request.CourseId);
        var course = await courseRepository.FirstOrDefaultAsync(new CourseByIdSpec(courseId), cancellationToken);
        if (course is null) return ApplicationResults.NotFound<NoContentResponse>(ErrorCode.CourseNotFound);
        if (await enrollmentRepository.AnyAsync(new ActiveEnrollmentUsingCourseSpec(courseId), cancellationToken))
            return ApplicationResults.Conflict<NoContentResponse>(ErrorCode.CatalogItemInUse);

        course.Deactivate(timeProvider.GetUtcNow());
        await courseRepository.UpdateAsync(course, cancellationToken);
        return Result<NoContentResponse>.NoContent();
    }
}
