using StudentManagementApi.Application.Common.Messaging;
using StudentManagementApi.Application.Contracts;

namespace StudentManagementApi.Application.Features.Enrollments.GetCurrentStudentEnrollment;

public sealed record GetCurrentStudentEnrollmentQuery : IQuery<EnrollmentResponse>;
