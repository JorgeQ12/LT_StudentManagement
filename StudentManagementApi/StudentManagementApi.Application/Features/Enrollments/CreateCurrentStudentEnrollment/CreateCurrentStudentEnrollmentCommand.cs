using StudentManagementApi.Application.Common.Messaging;
using StudentManagementApi.Application.Contracts;

namespace StudentManagementApi.Application.Features.Enrollments.CreateCurrentStudentEnrollment;

public sealed record CreateCurrentStudentEnrollmentCommand(Guid AcademicProgramId, IReadOnlyCollection<Guid> CourseIds)
    : ICommand<EnrollmentResponse>;
