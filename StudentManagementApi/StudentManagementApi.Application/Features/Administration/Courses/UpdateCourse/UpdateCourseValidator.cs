using FluentValidation;
using StudentManagementApi.Application.Common.Errors;
using StudentManagementApi.Application.Common.Validation;
using StudentManagementApi.Domain.Courses;
using StudentManagementApi.Domain.ValueObjects;

namespace StudentManagementApi.Application.Features.Administration.Courses.UpdateCourse;

internal sealed class UpdateCourseValidator : AbstractValidator<UpdateCourseCommand>
{
    public UpdateCourseValidator()
    {
        RuleFor(command => command.CourseId).NotEmpty().WithErrorCode(nameof(ErrorCode.InvalidIdentifier));
        RuleFor(command => command.Code).Required(CourseCode.MaximumLength, ErrorCode.InvalidCourseCode);
        RuleFor(command => command.Name).Required(Course.MaximumNameLength);
    }
}
