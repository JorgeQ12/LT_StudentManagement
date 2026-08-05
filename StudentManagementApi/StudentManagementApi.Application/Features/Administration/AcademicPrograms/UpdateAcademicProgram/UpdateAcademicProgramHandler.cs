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

namespace StudentManagementApi.Application.Features.Administration.AcademicPrograms.UpdateAcademicProgram;

internal sealed class UpdateAcademicProgramHandler(IWriteRepository<AcademicProgram> academicProgramRepository, TimeProvider timeProvider)
    : IRequestHandler<UpdateAcademicProgramCommand, Result<AcademicProgramResponse>>
{
    public async Task<Result<AcademicProgramResponse>> Handle(UpdateAcademicProgramCommand request, CancellationToken cancellationToken)
    {
        var programId = new AcademicProgramId(request.AcademicProgramId);
        var program = await academicProgramRepository.FirstOrDefaultAsync(new AcademicProgramByIdSpec(programId), cancellationToken);
        if (program is null)
        {
            return ApplicationResults.NotFound<AcademicProgramResponse>(ErrorCode.AcademicProgramNotFound);
        }

        var duplicate = await academicProgramRepository.FirstOrDefaultAsync(new AcademicProgramByCodeSpec(request.Code), cancellationToken);
        if (duplicate is not null && duplicate.Id != programId)
        {
            return ApplicationResults.Conflict<AcademicProgramResponse>(ErrorCode.AcademicProgramCodeAlreadyExists);
        }

        try
        {
            program.Update(ProgramCode.Create(request.Code), request.Name, request.Description, timeProvider.GetUtcNow());
            await academicProgramRepository.UpdateAsync(program, cancellationToken);
            return Result<AcademicProgramResponse>.Success(program.ToResponse());
        }
        catch (DomainRuleViolationException exception)
        {
            return DomainErrorMapper.ToResult<AcademicProgramResponse>(exception);
        }
    }
}
