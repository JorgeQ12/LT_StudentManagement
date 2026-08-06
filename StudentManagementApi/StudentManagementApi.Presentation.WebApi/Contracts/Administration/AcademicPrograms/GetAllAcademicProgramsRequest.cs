using StudentManagementApi.Domain;

namespace StudentManagementApi.Presentation.WebApi.Contracts.Administration.AcademicPrograms;

public sealed record GetAllAcademicProgramsRequest(
    int PageNumber = 1,
    int PageSize = 20,
    string? Search = null,
    CatalogStatus? Status = null);
