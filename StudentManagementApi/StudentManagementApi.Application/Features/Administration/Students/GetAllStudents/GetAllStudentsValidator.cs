using FluentValidation;
using StudentManagementApi.Application.Common.Errors;

namespace StudentManagementApi.Application.Features.Administration.Students.GetAllStudents;

internal sealed class GetAllStudentsValidator : AbstractValidator<GetAllStudentsQuery>
{
    public GetAllStudentsValidator()
    {
        RuleFor(query => query.PageNumber).GreaterThan(0).WithErrorCode(nameof(ErrorCode.ValidationFailed));
        RuleFor(query => query.PageSize).InclusiveBetween(1, 100).WithErrorCode(nameof(ErrorCode.ValidationFailed));
    }
}
