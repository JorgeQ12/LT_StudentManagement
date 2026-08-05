using StudentManagementApi.Application.Common.Messaging;
using StudentManagementApi.Application.Contracts;

namespace StudentManagementApi.Application.Features.Administration.Professors.GetProfessorById;

public sealed record GetProfessorByIdQuery(Guid ProfessorId) : IQuery<ProfessorResponse>;
