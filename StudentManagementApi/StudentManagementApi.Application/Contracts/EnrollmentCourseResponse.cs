namespace StudentManagementApi.Application.Contracts;

public sealed record EnrollmentCourseResponse(Guid Id, string Code, string Name, int Credits, Guid ProfessorId, string ProfessorFullName);
