namespace StudentManagementApi.Presentation.WebApi.Contracts.Enrollments;

public sealed record ReplaceSelectedCoursesRequest(IReadOnlyCollection<Guid> CourseIds);
