using Ardalis.Result;
using MediatR;
using StudentManagementApi.Application.Common.Errors;
using StudentManagementApi.Application.Common.Mappings;
using StudentManagementApi.Application.Common.Persistence;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Application.Specifications;
using StudentManagementApi.Domain.Professors;

namespace StudentManagementApi.Application.Features.Administration.Professors.ActivateProfessor;

internal sealed class ActivateProfessorHandler(IWriteRepository<Professor> professorRepository, TimeProvider timeProvider)
    : IRequestHandler<ActivateProfessorCommand, Result<ProfessorResponse>>
{
    public async Task<Result<ProfessorResponse>> Handle(ActivateProfessorCommand request, CancellationToken cancellationToken)
    {
        var professor = await professorRepository.FirstOrDefaultAsync(new ProfessorByIdSpec(new(request.ProfessorId)), cancellationToken);
        if (professor is null) return ApplicationResults.NotFound<ProfessorResponse>(ErrorCode.ProfessorNotFound);
        professor.Activate(timeProvider.GetUtcNow());
        await professorRepository.UpdateAsync(professor, cancellationToken);
        return Result<ProfessorResponse>.Success(professor.ToResponse());
    }
}
