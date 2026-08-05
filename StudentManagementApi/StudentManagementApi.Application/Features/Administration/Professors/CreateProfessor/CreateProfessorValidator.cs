using FluentValidation;
using StudentManagementApi.Application.Common.Errors;
using StudentManagementApi.Application.Common.Validation;
using StudentManagementApi.Domain.Professors;

namespace StudentManagementApi.Application.Features.Administration.Professors.CreateProfessor;

internal sealed class CreateProfessorValidator : AbstractValidator<CreateProfessorCommand>
{
    public CreateProfessorValidator()
    {
        RuleFor(command => command.FirstName).Required(Professor.MaximumNameLength);
        RuleFor(command => command.LastName).Required(Professor.MaximumNameLength);
    }
}
