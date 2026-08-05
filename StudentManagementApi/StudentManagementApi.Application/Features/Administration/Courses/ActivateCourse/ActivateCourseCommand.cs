using StudentManagementApi.Application.Common.Messaging;
using StudentManagementApi.Application.Contracts;

namespace StudentManagementApi.Application.Features.Administration.Courses.ActivateCourse;

public sealed record ActivateCourseCommand(Guid CourseId) : ICommand<CourseResponse>;
