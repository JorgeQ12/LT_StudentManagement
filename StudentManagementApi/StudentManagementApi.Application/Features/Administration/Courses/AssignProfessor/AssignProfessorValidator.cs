using FluentValidation;
using StudentManagementApi.Application.Common.Errors;

namespace StudentManagementApi.Application.Features.Administration.Courses.AssignProfessor;

internal sealed class AssignProfessorValidator : AbstractValidator<AssignProfessorCommand>
{
    public AssignProfessorValidator()
    {
        RuleFor(command => command.CourseId).NotEmpty().WithErrorCode(nameof(ErrorCode.InvalidIdentifier));
        RuleFor(command => command.ProfessorId).NotEmpty().WithErrorCode(nameof(ErrorCode.InvalidIdentifier));
    }
}
