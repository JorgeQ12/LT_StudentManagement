using StudentManagementApi.Application.Common.Messaging;
using StudentManagementApi.Application.Contracts;

namespace StudentManagementApi.Application.Features.Administration.AcademicPrograms.GetAcademicProgramById;

public sealed record GetAcademicProgramByIdQuery(Guid AcademicProgramId) : IQuery<AcademicProgramResponse>;
