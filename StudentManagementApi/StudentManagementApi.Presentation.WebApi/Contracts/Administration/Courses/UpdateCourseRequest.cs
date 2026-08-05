namespace StudentManagementApi.Presentation.WebApi.Contracts.Administration.Courses;

public sealed record UpdateCourseRequest(Guid CourseId, string Code, string Name);
