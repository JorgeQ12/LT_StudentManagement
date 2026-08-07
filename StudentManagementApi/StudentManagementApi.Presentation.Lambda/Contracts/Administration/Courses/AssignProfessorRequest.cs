namespace StudentManagementApi.Presentation.Lambda.Contracts.Administration.Courses;

public sealed record AssignProfessorRequest(Guid CourseId, Guid ProfessorId);
