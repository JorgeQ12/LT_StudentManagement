using Ardalis.Result;
using MediatR;
using StudentManagementApi.Application.Common.Errors;
using StudentManagementApi.Application.Common.Mappings;
using StudentManagementApi.Application.Common.Persistence;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Application.Specifications;
using StudentManagementApi.Domain.Professors;

namespace StudentManagementApi.Application.Features.Administration.Professors.GetProfessorById;

internal sealed class GetProfessorByIdHandler(IReadRepository<Professor> professorRepository)
    : IRequestHandler<GetProfessorByIdQuery, Result<ProfessorResponse>>
{
    public async Task<Result<ProfessorResponse>> Handle(GetProfessorByIdQuery request, CancellationToken cancellationToken)
    {
        var professor = await professorRepository.FirstOrDefaultAsync(new ProfessorByIdSpec(new(request.ProfessorId)), cancellationToken);
        return professor is null
            ? ApplicationResults.NotFound<ProfessorResponse>(ErrorCode.ProfessorNotFound)
            : Result<ProfessorResponse>.Success(professor.ToResponse());
    }
}
