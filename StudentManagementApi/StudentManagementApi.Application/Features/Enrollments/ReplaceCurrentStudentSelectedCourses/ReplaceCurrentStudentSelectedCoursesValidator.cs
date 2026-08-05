using FluentValidation;
using StudentManagementApi.Application.Common.Errors;
using StudentManagementApi.Domain.Enrollments;

namespace StudentManagementApi.Application.Features.Enrollments.ReplaceCurrentStudentSelectedCourses;

internal sealed class ReplaceCurrentStudentSelectedCoursesValidator : AbstractValidator<ReplaceCurrentStudentSelectedCoursesCommand>
{
    public ReplaceCurrentStudentSelectedCoursesValidator()
    {
        RuleFor(command => command.CourseIds)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithErrorCode(nameof(ErrorCode.EnrollmentMustContainThreeCourses))
            .Must(courseIds => courseIds.Count == Enrollment.RequiredCourseCount)
            .WithErrorCode(nameof(ErrorCode.EnrollmentMustContainThreeCourses));
        RuleFor(command => command.CourseIds)
            .Must(courseIds => courseIds.Distinct().Count() == courseIds.Count)
            .When(command => command.CourseIds is not null)
            .WithErrorCode(nameof(ErrorCode.EnrollmentCoursesMustBeDistinct));
        RuleForEach(command => command.CourseIds).NotEmpty().WithErrorCode(nameof(ErrorCode.InvalidIdentifier));
    }
}
