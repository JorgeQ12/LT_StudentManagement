using StudentManagementApi.Application.Common.Messaging;
using StudentManagementApi.Application.Contracts;

namespace StudentManagementApi.Application.Features.Administration.AcademicPrograms.ActivateAcademicProgram;

public sealed record ActivateAcademicProgramCommand(Guid AcademicProgramId) : ICommand<AcademicProgramResponse>;
