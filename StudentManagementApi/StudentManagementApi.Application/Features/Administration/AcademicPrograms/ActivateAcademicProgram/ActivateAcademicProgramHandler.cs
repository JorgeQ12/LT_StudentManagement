using Ardalis.Result;
using MediatR;
using StudentManagementApi.Application.Common.Errors;
using StudentManagementApi.Application.Common.Mappings;
using StudentManagementApi.Application.Common.Persistence;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Application.Specifications;
using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Programs;

namespace StudentManagementApi.Application.Features.Administration.AcademicPrograms.ActivateAcademicProgram;

internal sealed class ActivateAcademicProgramHandler(IWriteRepository<AcademicProgram> academicProgramRepository, TimeProvider timeProvider)
    : IRequestHandler<ActivateAcademicProgramCommand, Result<AcademicProgramResponse>>
{
    public async Task<Result<AcademicProgramResponse>> Handle(ActivateAcademicProgramCommand request, CancellationToken cancellationToken)
    {
        var program = await academicProgramRepository.FirstOrDefaultAsync(new AcademicProgramByIdSpec(new(request.AcademicProgramId)), cancellationToken);
        if (program is null)
        {
            return ApplicationResults.NotFound<AcademicProgramResponse>(ErrorCode.AcademicProgramNotFound);
        }

        program.Activate(timeProvider.GetUtcNow());
        await academicProgramRepository.UpdateAsync(program, cancellationToken);
        return Result<AcademicProgramResponse>.Success(program.ToResponse());
    }
}
