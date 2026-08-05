using StudentManagementApi.Application.Common.Messaging;
using StudentManagementApi.Application.Contracts;

namespace StudentManagementApi.Application.Features.Administration.Professors.GetAllProfessors;

public sealed record GetAllProfessorsQuery : IQuery<IReadOnlyCollection<ProfessorResponse>>;
