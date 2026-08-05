using FluentValidation;
using StudentManagementApi.Application.Common.Errors;
using StudentManagementApi.Application.Common.Validation;
using StudentManagementApi.Domain.Students;

namespace StudentManagementApi.Application.Features.Authentication.RegisterStudent;

internal sealed class RegisterStudentValidator : AbstractValidator<RegisterStudentCommand>
{
    public RegisterStudentValidator(TimeProvider timeProvider)
    {
        RuleFor(command => command.FirstName).Required(Student.MaximumNameLength);
        RuleFor(command => command.LastName).Required(Student.MaximumNameLength);
        RuleFor(command => command.DocumentNumber).ValidDocumentNumber();
        RuleFor(command => command.DateOfBirth).LessThan(DateOnly.FromDateTime(timeProvider.GetUtcNow().UtcDateTime))
            .WithErrorCode(nameof(ErrorCode.InvalidDateOfBirth));
        RuleFor(command => command.PhoneNumber).ValidPhoneNumber();
        RuleFor(command => command.Email).ValidEmail();
        RuleFor(command => command.Password).ValidPassword();
    }
}
