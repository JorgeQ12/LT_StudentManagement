using StudentManagementApi.Application.Common.Messaging;
using StudentManagementApi.Application.Contracts;

namespace StudentManagementApi.Application.Features.Administration.AcademicPrograms.GetAllAcademicPrograms;

public sealed record GetAllAcademicProgramsQuery : IQuery<IReadOnlyCollection<AcademicProgramResponse>>;
