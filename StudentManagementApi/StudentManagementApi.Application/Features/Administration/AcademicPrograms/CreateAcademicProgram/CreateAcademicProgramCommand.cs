using StudentManagementApi.Application.Common.Messaging;
using StudentManagementApi.Application.Contracts;

namespace StudentManagementApi.Application.Features.Administration.AcademicPrograms.CreateAcademicProgram;

public sealed record CreateAcademicProgramCommand(string Code, string Name, string Description) : ICommand<AcademicProgramResponse>;
