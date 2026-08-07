namespace StudentManagementApi.Presentation.Lambda.Contracts.Enrollments;

public sealed record CreateCurrentStudentEnrollmentRequest(Guid AcademicProgramId, IReadOnlyCollection<Guid> CourseIds);
