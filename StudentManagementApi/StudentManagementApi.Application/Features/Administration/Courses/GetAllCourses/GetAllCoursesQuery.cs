using StudentManagementApi.Application.Common.Messaging;
using StudentManagementApi.Application.Contracts;

namespace StudentManagementApi.Application.Features.Administration.Courses.GetAllCourses;

public sealed record GetAllCoursesQuery : IQuery<IReadOnlyCollection<CourseResponse>>;
