using StudentManagementApi.Application.Common.Messaging;
using StudentManagementApi.Application.Contracts;

namespace StudentManagementApi.Application.Features.Administration.Courses.UpdateCourse;

public sealed record UpdateCourseCommand(Guid CourseId, string Code, string Name) : ICommand<CourseResponse>;
