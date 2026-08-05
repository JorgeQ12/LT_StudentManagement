using Ardalis.Result;
using MediatR;
using StudentManagementApi.Application.Common.Errors;
using StudentManagementApi.Application.Common.Mappings;
using StudentManagementApi.Application.Common.Persistence;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Application.Specifications;
using StudentManagementApi.Domain.Courses;

namespace StudentManagementApi.Application.Features.Administration.Courses.ActivateCourse;

internal sealed class ActivateCourseHandler(IWriteRepository<Course> courseRepository, TimeProvider timeProvider)
    : IRequestHandler<ActivateCourseCommand, Result<CourseResponse>>
{
    public async Task<Result<CourseResponse>> Handle(ActivateCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await courseRepository.FirstOrDefaultAsync(new CourseByIdSpec(new(request.CourseId)), cancellationToken);
        if (course is null) return ApplicationResults.NotFound<CourseResponse>(ErrorCode.CourseNotFound);
        course.Activate(timeProvider.GetUtcNow());
        await courseRepository.UpdateAsync(course, cancellationToken);
        return Result<CourseResponse>.Success(course.ToResponse());
    }
}
