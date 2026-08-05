using StudentManagementApi.Application.Common.Messaging;
using StudentManagementApi.Application.Contracts;

namespace StudentManagementApi.Application.Features.Administration.AcademicPrograms.UpdateAcademicProgram;

public sealed record UpdateAcademicProgramCommand(Guid AcademicProgramId, string Code, string Name, string Description)
    : ICommand<AcademicProgramResponse>;
