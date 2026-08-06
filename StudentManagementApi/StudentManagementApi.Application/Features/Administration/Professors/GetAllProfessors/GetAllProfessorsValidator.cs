using FluentValidation;
using StudentManagementApi.Application.Common.Errors;

namespace StudentManagementApi.Application.Features.Administration.Professors.GetAllProfessors;

internal sealed class GetAllProfessorsValidator : AbstractValidator<GetAllProfessorsQuery>
{
    public GetAllProfessorsValidator()
    {
        RuleFor(query => query.PageNumber).GreaterThan(0).WithErrorCode(nameof(ErrorCode.ValidationFailed));
        RuleFor(query => query.PageSize).InclusiveBetween(1, 100).WithErrorCode(nameof(ErrorCode.ValidationFailed));
    }
}
