using StudentManagementApi.Domain.Abstractions;

namespace StudentManagementApi.Domain.Enrollments;

public sealed record CourseSelection(
    CourseId CourseId,
    AcademicProgramId AcademicProgramId,
    ProfessorId ProfessorId,
    CatalogStatus CourseStatus,
    CatalogStatus ProfessorStatus,
    int Credits);
