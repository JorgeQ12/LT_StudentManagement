using StudentManagementApi.Application.Common.Messaging;

namespace StudentManagementApi.Application.Features.Enrollments.CancelCurrentStudentEnrollment;

public sealed record CancelCurrentStudentEnrollmentCommand : ICommand<NoContentResponse>;
