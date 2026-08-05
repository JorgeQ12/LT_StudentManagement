using FluentValidation;
using StudentManagementApi.Application.Common.Errors;
using StudentManagementApi.Application.Common.Validation;
using StudentManagementApi.Domain.Courses;
using StudentManagementApi.Domain.ValueObjects;

namespace StudentManagementApi.Application.Features.Administration.Courses.CreateCourse;

internal sealed class CreateCourseValidator : AbstractValidator<CreateCourseCommand>
{
    public CreateCourseValidator()
    {
        RuleFor(command => command.AcademicProgramId).NotEmpty().WithErrorCode(nameof(ErrorCode.InvalidIdentifier));
        RuleFor(command => command.Code).Required(CourseCode.MaximumLength, ErrorCode.InvalidCourseCode);
        RuleFor(command => command.Name).Required(Course.MaximumNameLength);
    }
}
