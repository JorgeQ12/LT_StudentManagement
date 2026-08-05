using Ardalis.Result;
using MediatR;
using StudentManagementApi.Application.Common.Errors;
using StudentManagementApi.Application.Common.Persistence;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Application.Specifications;
using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Programs;

namespace StudentManagementApi.Application.Features.Administration.AcademicPrograms.GetAcademicProgramById;

internal sealed class GetAcademicProgramByIdHandler(IReadRepository<AcademicProgram> academicProgramRepository)
    : IRequestHandler<GetAcademicProgramByIdQuery, Result<AcademicProgramResponse>>
{
    public async Task<Result<AcademicProgramResponse>> Handle(GetAcademicProgramByIdQuery request, CancellationToken cancellationToken)
    {
        var response = await academicProgramRepository.FirstOrDefaultAsync(new AcademicProgramResponseByIdSpec(new(request.AcademicProgramId)),
            cancellationToken);
        return response is null
            ? ApplicationResults.NotFound<AcademicProgramResponse>(ErrorCode.AcademicProgramNotFound)
            : Result<AcademicProgramResponse>.Success(response);
    }
}
