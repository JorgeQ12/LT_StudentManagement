using StudentManagementApi.Domain;

namespace StudentManagementApi.Application.Contracts;

public sealed record EnrollmentResponse(
    Guid Id,
    Guid StudentId,
    AcademicProgramResponse AcademicProgram,
    EnrollmentStatus Status,
    int TotalCredits,
    IReadOnlyCollection<EnrollmentCourseResponse> Courses,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? CancelledAtUtc);
