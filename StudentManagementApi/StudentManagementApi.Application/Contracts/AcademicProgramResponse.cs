using StudentManagementApi.Domain;

namespace StudentManagementApi.Application.Contracts;

public sealed record AcademicProgramResponse(Guid Id, string Code, string Name, string Description, CatalogStatus Status);
