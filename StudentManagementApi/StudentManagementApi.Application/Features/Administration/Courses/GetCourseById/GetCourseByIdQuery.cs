using StudentManagementApi.Application.Common.Messaging;
using StudentManagementApi.Application.Contracts;

namespace StudentManagementApi.Application.Features.Administration.Courses.GetCourseById;

public sealed record GetCourseByIdQuery(Guid CourseId) : IQuery<CourseResponse>;
