using FluentValidation;
using StudentManagementApi.Application.Common.Errors;

namespace StudentManagementApi.Application.Features.AcademicCatalog.GetActiveCoursesByAcademicProgram;

internal sealed class GetActiveCoursesByAcademicProgramValidator : AbstractValidator<GetActiveCoursesByAcademicProgramQuery>
{
    public GetActiveCoursesByAcademicProgramValidator() =>
        RuleFor(query => query.AcademicProgramId).NotEmpty().WithErrorCode(nameof(ErrorCode.InvalidIdentifier));
}
