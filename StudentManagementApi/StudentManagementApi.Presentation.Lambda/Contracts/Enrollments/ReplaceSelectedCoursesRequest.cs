namespace StudentManagementApi.Presentation.Lambda.Contracts.Enrollments;

public sealed record ReplaceSelectedCoursesRequest(IReadOnlyCollection<Guid> CourseIds);
