using FluentValidation;
using StudentManagementApi.Application.Common.Errors;
using StudentManagementApi.Application.Common.Validation;
using StudentManagementApi.Domain.Students;

namespace StudentManagementApi.Application.Features.Administration.Students.CreateStudent;

internal sealed class CreateStudentValidator : AbstractValidator<CreateStudentCommand>
{
    public CreateStudentValidator(TimeProvider timeProvider)
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
