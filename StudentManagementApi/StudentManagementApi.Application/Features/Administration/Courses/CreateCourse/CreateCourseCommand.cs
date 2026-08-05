using StudentManagementApi.Application.Common.Messaging;
using StudentManagementApi.Application.Contracts;

namespace StudentManagementApi.Application.Features.Administration.Courses.CreateCourse;

public sealed record CreateCourseCommand(Guid AcademicProgramId, string Code, string Name) : ICommand<CourseResponse>;
