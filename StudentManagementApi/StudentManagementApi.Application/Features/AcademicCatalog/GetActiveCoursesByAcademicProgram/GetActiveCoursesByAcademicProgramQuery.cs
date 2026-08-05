using StudentManagementApi.Application.Common.Messaging;
using StudentManagementApi.Application.Contracts;

namespace StudentManagementApi.Application.Features.AcademicCatalog.GetActiveCoursesByAcademicProgram;

public sealed record GetActiveCoursesByAcademicProgramQuery(Guid AcademicProgramId) : IQuery<IReadOnlyCollection<CourseResponse>>;
