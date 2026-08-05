using StudentManagementApi.Application.Common.Messaging;
using StudentManagementApi.Application.Contracts;

namespace StudentManagementApi.Application.Features.AcademicCatalog.GetActiveAcademicPrograms;

public sealed record GetActiveAcademicProgramsQuery : IQuery<IReadOnlyCollection<AcademicProgramResponse>>;
