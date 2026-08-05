using StudentManagementApi.Domain;

namespace StudentManagementApi.Application.Contracts;

public sealed record CourseResponse(
    Guid Id,
    Guid AcademicProgramId,
    string Code,
    string Name,
    int Credits,
    CatalogStatus Status,
    Guid? ProfessorId,
    string? ProfessorFullName);
