using FluentValidation;
using StudentManagementApi.Application.Common.Errors;
using StudentManagementApi.Domain.ValueObjects;

namespace StudentManagementApi.Application.Common.Validation;

internal static class ValidationRules
{
    public static IRuleBuilderOptions<T, string> Required<T>(this IRuleBuilderInitial<T, string> rule, int maximumLength,
        ErrorCode lengthErrorCode = ErrorCode.ValueTooLong) => rule.Cascade(CascadeMode.Stop)
        .NotEmpty().WithErrorCode(nameof(ErrorCode.RequiredField))
        .MaximumLength(maximumLength).WithErrorCode(lengthErrorCode.ToString());

    public static IRuleBuilderOptions<T, string> ValidEmail<T>(this IRuleBuilderInitial<T, string> rule) => rule.Cascade(CascadeMode.Stop)
        .NotEmpty().WithErrorCode(nameof(ErrorCode.RequiredField))
        .MaximumLength(Email.MaximumLength).WithErrorCode(nameof(ErrorCode.InvalidEmail))
        .EmailAddress().WithErrorCode(nameof(ErrorCode.InvalidEmail));

    public static IRuleBuilderOptions<T, string> ValidDocumentNumber<T>(this IRuleBuilderInitial<T, string> rule) => rule.Cascade(CascadeMode.Stop)
        .NotEmpty().WithErrorCode(nameof(ErrorCode.RequiredField))
        .Length(5, DocumentNumber.MaximumLength).WithErrorCode(nameof(ErrorCode.InvalidDocumentNumber));

    public static IRuleBuilderOptions<T, string> ValidPhoneNumber<T>(this IRuleBuilderInitial<T, string> rule) => rule.Cascade(CascadeMode.Stop)
        .NotEmpty().WithErrorCode(nameof(ErrorCode.RequiredField))
        .Must(value => value.Length <= PhoneNumber.MaximumLength && value.Count(char.IsDigit) is >= 7 and <= 15)
        .WithErrorCode(nameof(ErrorCode.InvalidPhoneNumber));

    public static IRuleBuilderOptions<T, string> ValidPassword<T>(this IRuleBuilderInitial<T, string> rule) => rule.Cascade(CascadeMode.Stop)
        .NotEmpty().WithErrorCode(nameof(ErrorCode.RequiredField))
        .MinimumLength(10).WithErrorCode(nameof(ErrorCode.InvalidPassword))
        .Matches("[A-Z]").WithErrorCode(nameof(ErrorCode.InvalidPassword))
        .Matches("[a-z]").WithErrorCode(nameof(ErrorCode.InvalidPassword))
        .Matches("[0-9]").WithErrorCode(nameof(ErrorCode.InvalidPassword));
}
