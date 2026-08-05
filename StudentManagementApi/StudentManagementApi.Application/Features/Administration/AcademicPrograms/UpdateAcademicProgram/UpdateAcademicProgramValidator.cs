using FluentValidation;
using StudentManagementApi.Application.Common.Errors;
using StudentManagementApi.Application.Common.Validation;
using StudentManagementApi.Domain.Programs;
using StudentManagementApi.Domain.ValueObjects;

namespace StudentManagementApi.Application.Features.Administration.AcademicPrograms.UpdateAcademicProgram;

internal sealed class UpdateAcademicProgramValidator : AbstractValidator<UpdateAcademicProgramCommand>
{
    public UpdateAcademicProgramValidator()
    {
        RuleFor(command => command.AcademicProgramId).NotEmpty().WithErrorCode(nameof(ErrorCode.InvalidIdentifier));
        RuleFor(command => command.Code).Required(ProgramCode.MaximumLength, ErrorCode.InvalidProgramCode);
        RuleFor(command => command.Name).Required(AcademicProgram.MaximumNameLength);
        RuleFor(command => command.Description).Required(AcademicProgram.MaximumDescriptionLength);
    }
}
