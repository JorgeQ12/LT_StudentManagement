namespace StudentManagementApi.Presentation.Lambda.Contracts.Administration.Courses;

public sealed record UpdateCourseRequest(Guid CourseId, string Code, string Name);
