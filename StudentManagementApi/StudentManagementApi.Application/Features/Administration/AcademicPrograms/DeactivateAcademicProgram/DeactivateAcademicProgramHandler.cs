using Ardalis.Result;
using MediatR;
using StudentManagementApi.Application.Common.Errors;
using StudentManagementApi.Application.Common.Messaging;
using StudentManagementApi.Application.Common.Persistence;
using StudentManagementApi.Application.Specifications;
using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Enrollments;
using StudentManagementApi.Domain.Programs;

namespace StudentManagementApi.Application.Features.Administration.AcademicPrograms.DeactivateAcademicProgram;

internal sealed class DeactivateAcademicProgramHandler(IWriteRepository<AcademicProgram> academicProgramRepository,
    IWriteRepository<Enrollment> enrollmentRepository, TimeProvider timeProvider)
    : IRequestHandler<DeactivateAcademicProgramCommand, Result<NoContentResponse>>
{
    public async Task<Result<NoContentResponse>> Handle(DeactivateAcademicProgramCommand request, CancellationToken cancellationToken)
    {
        var programId = new AcademicProgramId(request.AcademicProgramId);
        var program = await academicProgramRepository.FirstOrDefaultAsync(new AcademicProgramByIdSpec(programId), cancellationToken);
        if (program is null)
        {
            return ApplicationResults.NotFound<NoContentResponse>(ErrorCode.AcademicProgramNotFound);
        }

        if (await enrollmentRepository.AnyAsync(new ActiveEnrollmentInProgramSpec(programId), cancellationToken))
        {
            return ApplicationResults.Conflict<NoContentResponse>(ErrorCode.CatalogItemInUse);
        }

        program.Deactivate(timeProvider.GetUtcNow());
        await academicProgramRepository.UpdateAsync(program, cancellationToken);
        return Result<NoContentResponse>.NoContent();
    }
}
