using FluentValidation;
using StudentManagementApi.Application.Common.Errors;
using StudentManagementApi.Application.Common.Validation;
using StudentManagementApi.Domain.Professors;

namespace StudentManagementApi.Application.Features.Administration.Professors.UpdateProfessor;

internal sealed class UpdateProfessorValidator : AbstractValidator<UpdateProfessorCommand>
{
    public UpdateProfessorValidator()
    {
        RuleFor(command => command.ProfessorId).NotEmpty().WithErrorCode(nameof(ErrorCode.InvalidIdentifier));
        RuleFor(command => command.FirstName).Required(Professor.MaximumNameLength);
        RuleFor(command => command.LastName).Required(Professor.MaximumNameLength);
    }
}
