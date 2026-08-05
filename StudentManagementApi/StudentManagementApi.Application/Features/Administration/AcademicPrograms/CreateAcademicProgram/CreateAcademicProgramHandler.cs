using Ardalis.Result;
using MediatR;
using StudentManagementApi.Application.Common.Errors;
using StudentManagementApi.Application.Common.Mappings;
using StudentManagementApi.Application.Common.Persistence;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Application.Specifications;
using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Programs;
using StudentManagementApi.Domain.ValueObjects;

namespace StudentManagementApi.Application.Features.Administration.AcademicPrograms.CreateAcademicProgram;

internal sealed class CreateAcademicProgramHandler(IWriteRepository<AcademicProgram> academicProgramRepository, TimeProvider timeProvider)
    : IRequestHandler<CreateAcademicProgramCommand, Result<AcademicProgramResponse>>
{
    public async Task<Result<AcademicProgramResponse>> Handle(CreateAcademicProgramCommand request, CancellationToken cancellationToken)
    {
        if (await academicProgramRepository.AnyAsync(new AcademicProgramByCodeSpec(request.Code), cancellationToken))
        {
            return ApplicationResults.Conflict<AcademicProgramResponse>(ErrorCode.AcademicProgramCodeAlreadyExists);
        }

        try
        {
            var program = AcademicProgram.Create(AcademicProgramId.New(), ProgramCode.Create(request.Code), request.Name, request.Description,
                timeProvider.GetUtcNow());
            await academicProgramRepository.AddAsync(program, cancellationToken);
            return Result<AcademicProgramResponse>.Created(program.ToResponse());
        }
        catch (DomainRuleViolationException exception)
        {
            return DomainErrorMapper.ToResult<AcademicProgramResponse>(exception);
        }
    }
}
