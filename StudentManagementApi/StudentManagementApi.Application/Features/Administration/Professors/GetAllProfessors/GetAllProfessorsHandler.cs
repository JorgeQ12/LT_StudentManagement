using Ardalis.Result;
using MediatR;
using StudentManagementApi.Application.Common.Persistence;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Application.Specifications;
using StudentManagementApi.Domain.Professors;

namespace StudentManagementApi.Application.Features.Administration.Professors.GetAllProfessors;

internal sealed class GetAllProfessorsHandler(IReadRepository<Professor> professorRepository)
    : IRequestHandler<GetAllProfessorsQuery, Result<PagedResponse<ProfessorResponse>>>
{
    public async Task<Result<PagedResponse<ProfessorResponse>>> Handle(
        GetAllProfessorsQuery request,
        CancellationToken cancellationToken)
    {
        var search = string.IsNullOrWhiteSpace(request.Search) ? null : request.Search.Trim();

        var totalCount = await professorRepository.CountAsync(
            new ProfessorsCountSpec(search, request.Status),
            cancellationToken);
        var professors = await professorRepository.ListAsync(
            new ProfessorsPageSpec(request.PageNumber, request.PageSize, search, request.Status),
            cancellationToken);

        return Result<PagedResponse<ProfessorResponse>>.Success(new(
            professors,
            request.PageNumber,
            request.PageSize,
            totalCount));
    }
}
