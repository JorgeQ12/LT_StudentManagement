using FluentValidation;
using StudentManagementApi.Application.Common.Errors;
using StudentManagementApi.Application.Common.Validation;
using StudentManagementApi.Domain.Programs;
using StudentManagementApi.Domain.ValueObjects;

namespace StudentManagementApi.Application.Features.Administration.AcademicPrograms.CreateAcademicProgram;

internal sealed class CreateAcademicProgramValidator : AbstractValidator<CreateAcademicProgramCommand>
{
    public CreateAcademicProgramValidator()
    {
        RuleFor(command => command.Code).Required(ProgramCode.MaximumLength, ErrorCode.InvalidProgramCode);
        RuleFor(command => command.Name).Required(AcademicProgram.MaximumNameLength);
        RuleFor(command => command.Description).Required(AcademicProgram.MaximumDescriptionLength);
    }
}
