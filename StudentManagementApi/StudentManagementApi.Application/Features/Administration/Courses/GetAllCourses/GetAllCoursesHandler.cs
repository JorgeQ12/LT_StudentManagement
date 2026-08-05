using Ardalis.Result;
using MediatR;
using StudentManagementApi.Application.Common.Mappings;
using StudentManagementApi.Application.Common.Persistence;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Application.Specifications;
using StudentManagementApi.Domain.Courses;

namespace StudentManagementApi.Application.Features.Administration.Courses.GetAllCourses;

internal sealed class GetAllCoursesHandler(IReadRepository<Course> courseRepository)
    : IRequestHandler<GetAllCoursesQuery, Result<IReadOnlyCollection<CourseResponse>>>
{
    public async Task<Result<IReadOnlyCollection<CourseResponse>>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
    {
        var courses = await courseRepository.ListAsync(new CoursesSpec(), cancellationToken);
        return Result<IReadOnlyCollection<CourseResponse>>.Success(courses.Select(course => course.ToResponse()).ToArray());
    }
}
