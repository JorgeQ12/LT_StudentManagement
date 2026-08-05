using FluentValidation;
using StudentManagementApi.Application.Common.Errors;
using StudentManagementApi.Application.Common.Validation;
using StudentManagementApi.Domain.Students;

namespace StudentManagementApi.Application.Features.Administration.Students.UpdateStudent;

internal sealed class UpdateStudentValidator : AbstractValidator<UpdateStudentCommand>
{
    public UpdateStudentValidator(TimeProvider timeProvider)
    {
        RuleFor(command => command.StudentId).NotEmpty().WithErrorCode(nameof(ErrorCode.InvalidIdentifier));
        RuleFor(command => command.FirstName).Required(Student.MaximumNameLength);
        RuleFor(command => command.LastName).Required(Student.MaximumNameLength);
        RuleFor(command => command.DocumentNumber).ValidDocumentNumber();
        RuleFor(command => command.DateOfBirth).LessThan(DateOnly.FromDateTime(timeProvider.GetUtcNow().UtcDateTime))
            .WithErrorCode(nameof(ErrorCode.InvalidDateOfBirth));
        RuleFor(command => command.PhoneNumber).ValidPhoneNumber();
        RuleFor(command => command.Email).ValidEmail();
    }
}
