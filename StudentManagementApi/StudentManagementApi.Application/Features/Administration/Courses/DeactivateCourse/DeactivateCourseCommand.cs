using StudentManagementApi.Application.Common.Messaging;

namespace StudentManagementApi.Application.Features.Administration.Courses.DeactivateCourse;

public sealed record DeactivateCourseCommand(Guid CourseId) : ICommand<NoContentResponse>;
