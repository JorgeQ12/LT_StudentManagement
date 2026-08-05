using Ardalis.Result;
using MediatR;
using StudentManagementApi.Application.Common.Errors;
using StudentManagementApi.Application.Common.Mappings;
using StudentManagementApi.Application.Common.Persistence;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Application.Specifications;
using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Professors;

namespace StudentManagementApi.Application.Features.Administration.Professors.UpdateProfessor;

internal sealed class UpdateProfessorHandler(IWriteRepository<Professor> professorRepository, TimeProvider timeProvider)
    : IRequestHandler<UpdateProfessorCommand, Result<ProfessorResponse>>
{
    public async Task<Result<ProfessorResponse>> Handle(UpdateProfessorCommand request, CancellationToken cancellationToken)
    {
        var professor = await professorRepository.FirstOrDefaultAsync(new ProfessorByIdSpec(new(request.ProfessorId)), cancellationToken);
        if (professor is null) return ApplicationResults.NotFound<ProfessorResponse>(ErrorCode.ProfessorNotFound);

        try
        {
            professor.Update(request.FirstName, request.LastName, timeProvider.GetUtcNow());
            await professorRepository.UpdateAsync(professor, cancellationToken);
            return Result<ProfessorResponse>.Success(professor.ToResponse());
        }
        catch (DomainRuleViolationException exception)
        {
            return DomainErrorMapper.ToResult<ProfessorResponse>(exception);
        }
    }
}
