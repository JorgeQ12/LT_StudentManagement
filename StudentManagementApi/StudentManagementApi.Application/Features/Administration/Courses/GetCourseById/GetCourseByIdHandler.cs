using Ardalis.Result;
using MediatR;
using StudentManagementApi.Application.Common.Errors;
using StudentManagementApi.Application.Common.Mappings;
using StudentManagementApi.Application.Common.Persistence;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Application.Specifications;
using StudentManagementApi.Domain.Courses;

namespace StudentManagementApi.Application.Features.Administration.Courses.GetCourseById;

internal sealed class GetCourseByIdHandler(IReadRepository<Course> courseRepository) : IRequestHandler<GetCourseByIdQuery, Result<CourseResponse>>
{
    public async Task<Result<CourseResponse>> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
    {
        var course = await courseRepository.FirstOrDefaultAsync(new CourseByIdSpec(new(request.CourseId)), cancellationToken);
        return course is null ? ApplicationResults.NotFound<CourseResponse>(ErrorCode.CourseNotFound) : Result<CourseResponse>.Success(course.ToResponse());
    }
}
