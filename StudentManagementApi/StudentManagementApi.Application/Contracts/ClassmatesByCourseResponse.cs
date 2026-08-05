namespace StudentManagementApi.Application.Contracts;

public sealed record ClassmatesByCourseResponse(Guid CourseId, string CourseName, IReadOnlyCollection<ClassmateResponse> Classmates);
