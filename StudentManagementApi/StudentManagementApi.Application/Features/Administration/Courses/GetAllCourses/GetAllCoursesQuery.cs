using StudentManagementApi.Application.Common.Messaging;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Domain;

namespace StudentManagementApi.Application.Features.Administration.Courses.GetAllCourses;

public sealed record GetAllCoursesQuery(
    int PageNumber = 1,
    int PageSize = 20,
    string? Search = null,
    CatalogStatus? Status = null,
    Guid? AcademicProgramId = null) : IQuery<PagedResponse<CourseResponse>>;
