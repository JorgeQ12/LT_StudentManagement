using FluentValidation;
using StudentManagementApi.Application.Common.Errors;
using StudentManagementApi.Application.Common.Validation;

namespace StudentManagementApi.Application.Features.Authentication.Login;

internal sealed class LoginValidator : AbstractValidator<LoginCommand>
{
    public LoginValidator()
    {
        RuleFor(command => command.Email).ValidEmail();
        RuleFor(command => command.Password).NotEmpty().WithErrorCode(nameof(ErrorCode.RequiredField));
    }
}
