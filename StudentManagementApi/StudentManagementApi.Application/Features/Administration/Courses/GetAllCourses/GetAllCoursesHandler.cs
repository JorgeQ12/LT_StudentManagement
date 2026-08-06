using Ardalis.Result;
using MediatR;
using StudentManagementApi.Application.Common.Mappings;
using StudentManagementApi.Application.Common.Persistence;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Application.Specifications;
using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Courses;

namespace StudentManagementApi.Application.Features.Administration.Courses.GetAllCourses;

internal sealed class GetAllCoursesHandler(IReadRepository<Course> courseRepository)
    : IRequestHandler<GetAllCoursesQuery, Result<PagedResponse<CourseResponse>>>
{
    public async Task<Result<PagedResponse<CourseResponse>>> Handle(
        GetAllCoursesQuery request,
        CancellationToken cancellationToken)
    {
        var search = string.IsNullOrWhiteSpace(request.Search) ? null : request.Search.Trim();
        AcademicProgramId? academicProgramId = request.AcademicProgramId is null
            ? null
            : new AcademicProgramId(request.AcademicProgramId.Value);

        var totalCount = await courseRepository.CountAsync(
            new CoursesCountSpec(search, request.Status, academicProgramId),
            cancellationToken);
        var courses = await courseRepository.ListAsync(
            new CoursesPageSpec(request.PageNumber, request.PageSize, search, request.Status, academicProgramId),
            cancellationToken);

        return Result<PagedResponse<CourseResponse>>.Success(new(
            courses.Select(course => course.ToResponse()).ToArray(),
            request.PageNumber,
            request.PageSize,
            totalCount));
    }
}
