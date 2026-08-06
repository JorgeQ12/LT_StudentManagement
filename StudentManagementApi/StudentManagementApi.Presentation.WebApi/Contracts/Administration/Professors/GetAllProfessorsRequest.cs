using StudentManagementApi.Domain;

namespace StudentManagementApi.Presentation.WebApi.Contracts.Administration.Professors;

public sealed record GetAllProfessorsRequest(
    int PageNumber = 1,
    int PageSize = 20,
    string? Search = null,
    CatalogStatus? Status = null);
