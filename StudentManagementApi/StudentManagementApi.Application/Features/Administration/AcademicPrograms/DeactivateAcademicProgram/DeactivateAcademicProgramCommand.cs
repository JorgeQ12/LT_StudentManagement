using StudentManagementApi.Application.Common.Messaging;

namespace StudentManagementApi.Application.Features.Administration.AcademicPrograms.DeactivateAcademicProgram;

public sealed record DeactivateAcademicProgramCommand(Guid AcademicProgramId) : ICommand<NoContentResponse>;
