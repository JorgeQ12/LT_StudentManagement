using StudentManagementApi.Domain;

namespace StudentManagementApi.Presentation.WebApi.Contracts.Administration.Students;

public sealed record GetAllStudentsRequest(
    int PageNumber = 1,
    int PageSize = 20,
    string? Search = null,
    AccountStatus? Status = null);