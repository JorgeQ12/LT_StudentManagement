using FluentValidation;
using StudentManagementApi.Application.Common.Errors;
using StudentManagementApi.Application.Common.Validation;
using StudentManagementApi.Domain.Students;

namespace StudentManagementApi.Application.Features.StudentProfiles.UpdateCurrentStudentProfile;

internal sealed class UpdateCurrentStudentProfileValidator
    : AbstractValidator<UpdateCurrentStudentProfileCommand>
{
    public UpdateCurrentStudentProfileValidator(TimeProvider timeProvider)
    {
        RuleFor(command => command.FirstName).Required(Student.MaximumNameLength);
        RuleFor(command => command.LastName).Required(Student.MaximumNameLength);
        RuleFor(command => command.DateOfBirth).LessThan(DateOnly.FromDateTime(timeProvider.GetUtcNow().UtcDateTime))
            .WithErrorCode(nameof(ErrorCode.InvalidDateOfBirth));
        RuleFor(command => command.PhoneNumber).ValidPhoneNumber();
        RuleFor(command => command.Email).ValidEmail();
    }
}
