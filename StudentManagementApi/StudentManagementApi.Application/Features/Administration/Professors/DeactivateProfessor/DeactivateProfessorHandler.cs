using Ardalis.Result;
using MediatR;
using StudentManagementApi.Application.Common.Errors;
using StudentManagementApi.Application.Common.Messaging;
using StudentManagementApi.Application.Common.Persistence;
using StudentManagementApi.Application.Specifications;
using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Professors;

namespace StudentManagementApi.Application.Features.Administration.Professors.DeactivateProfessor;

internal sealed class DeactivateProfessorHandler(IWriteRepository<Professor> professorRepository, TimeProvider timeProvider)
    : IRequestHandler<DeactivateProfessorCommand, Result<NoContentResponse>>
{
    public async Task<Result<NoContentResponse>> Handle(DeactivateProfessorCommand request, CancellationToken cancellationToken)
    {
        var professor = await professorRepository.FirstOrDefaultAsync(new ProfessorByIdSpec(new(request.ProfessorId)), cancellationToken);
        if (professor is null) return ApplicationResults.NotFound<NoContentResponse>(ErrorCode.ProfessorNotFound);

        try
        {
            professor.Deactivate(timeProvider.GetUtcNow());
            await professorRepository.UpdateAsync(professor, cancellationToken);
            return Result<NoContentResponse>.NoContent();
        }
        catch (DomainRuleViolationException exception)
        {
            return DomainErrorMapper.ToResult<NoContentResponse>(exception);
        }
    }
}
