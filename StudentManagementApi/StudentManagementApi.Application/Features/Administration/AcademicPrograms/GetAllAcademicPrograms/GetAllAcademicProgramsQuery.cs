using StudentManagementApi.Application.Common.Messaging;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Domain;

namespace StudentManagementApi.Application.Features.Administration.AcademicPrograms.GetAllAcademicPrograms;

public sealed record GetAllAcademicProgramsQuery(
    int PageNumber = 1,
    int PageSize = 20,
    string? Search = null,
    CatalogStatus? Status = null) : IQuery<PagedResponse<AcademicProgramResponse>>;
