using StudentManagementApi.Domain;

namespace StudentManagementApi.Presentation.WebApi.Contracts.Administration.Courses;

public sealed record GetAllCoursesRequest(
    int PageNumber = 1,
    int PageSize = 20,
    string? Search = null,
    CatalogStatus? Status = null,
    Guid? AcademicProgramId = null);
