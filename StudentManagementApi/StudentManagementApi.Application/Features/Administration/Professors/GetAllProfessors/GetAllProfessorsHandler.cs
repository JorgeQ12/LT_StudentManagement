using Ardalis.Result;
using MediatR;
using StudentManagementApi.Application.Common.Mappings;
using StudentManagementApi.Application.Common.Persistence;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Application.Specifications;
using StudentManagementApi.Domain.Professors;

namespace StudentManagementApi.Application.Features.Administration.Professors.GetAllProfessors;

internal sealed class GetAllProfessorsHandler(IReadRepository<Professor> professorRepository)
    : IRequestHandler<GetAllProfessorsQuery, Result<IReadOnlyCollection<ProfessorResponse>>>
{
    public async Task<Result<IReadOnlyCollection<ProfessorResponse>>> Handle(GetAllProfessorsQuery request, CancellationToken cancellationToken)
    {
        var professors = await professorRepository.ListAsync(new ProfessorsSpec(), cancellationToken);
        return Result<IReadOnlyCollection<ProfessorResponse>>.Success(professors.Select(professor => professor.ToResponse()).ToArray());
    }
}
