using StudentManagementApi.Domain;

namespace StudentManagementApi.Application.Contracts;

public sealed record ProfessorResponse(Guid Id, string FirstName, string LastName, CatalogStatus Status, int AssignedCourseCount);
