using StudentManagementApi.Application.Common.Messaging;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Domain;

namespace StudentManagementApi.Application.Features.Administration.Professors.GetAllProfessors;

public sealed record GetAllProfessorsQuery(
    int PageNumber = 1,
    int PageSize = 20,
    string? Search = null,
    CatalogStatus? Status = null) : IQuery<PagedResponse<ProfessorResponse>>;
