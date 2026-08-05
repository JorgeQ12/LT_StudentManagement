namespace StudentManagementApi.Presentation.WebApi.Contracts.Enrollments;

public sealed record CreateCurrentStudentEnrollmentRequest(Guid AcademicProgramId, IReadOnlyCollection<Guid> CourseIds);
