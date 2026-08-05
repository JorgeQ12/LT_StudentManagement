using StudentManagementApi.Application.Common.Messaging;
using StudentManagementApi.Application.Contracts;

namespace StudentManagementApi.Application.Features.Enrollments.ReplaceCurrentStudentSelectedCourses;

public sealed record ReplaceCurrentStudentSelectedCoursesCommand(IReadOnlyCollection<Guid> CourseIds) : ICommand<EnrollmentResponse>;
