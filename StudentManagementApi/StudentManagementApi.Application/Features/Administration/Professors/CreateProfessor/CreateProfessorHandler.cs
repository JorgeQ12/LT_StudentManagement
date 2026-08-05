using Ardalis.Result;
using MediatR;
using StudentManagementApi.Application.Common.Errors;
using StudentManagementApi.Application.Common.Mappings;
using StudentManagementApi.Application.Common.Persistence;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Professors;

namespace StudentManagementApi.Application.Features.Administration.Professors.CreateProfessor;

internal sealed class CreateProfessorHandler(IWriteRepository<Professor> professorRepository, TimeProvider timeProvider)
    : IRequestHandler<CreateProfessorCommand, Result<ProfessorResponse>>
{
    public async Task<Result<ProfessorResponse>> Handle(CreateProfessorCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var professor = Professor.Create(ProfessorId.New(), request.FirstName, request.LastName, timeProvider.GetUtcNow());
            await professorRepository.AddAsync(professor, cancellationToken);
            return Result<ProfessorResponse>.Created(professor.ToResponse());
        }
        catch (DomainRuleViolationException exception)
        {
            return DomainErrorMapper.ToResult<ProfessorResponse>(exception);
        }
    }
}
