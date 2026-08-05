using Ardalis.Result;
using StudentManagementApi.Domain;
using StudentManagementApi.Domain.Abstractions;

namespace StudentManagementApi.Application.Common.Errors;

internal static class DomainErrorMapper
{
    public static Result<T> ToResult<T>(DomainRuleViolationException exception)
    {
        var code = exception.Code switch
        {
            DomainRuleCode.RequiredValue => ErrorCode.RequiredField,
            DomainRuleCode.ValueExceedsMaximumLength => ErrorCode.ValueTooLong,
            DomainRuleCode.InvalidEmail => ErrorCode.InvalidEmail,
            DomainRuleCode.InvalidDocumentNumber => ErrorCode.InvalidDocumentNumber,
            DomainRuleCode.InvalidPhoneNumber => ErrorCode.InvalidPhoneNumber,
            DomainRuleCode.InvalidDateOfBirth => ErrorCode.InvalidDateOfBirth,
            DomainRuleCode.InvalidProgramCode => ErrorCode.InvalidProgramCode,
            DomainRuleCode.InvalidCourseCode => ErrorCode.InvalidCourseCode,
            DomainRuleCode.AccountIsInactive => ErrorCode.AccountInactive,
            DomainRuleCode.CatalogItemIsInactive => ErrorCode.CatalogItemInactive,
            DomainRuleCode.ProfessorCourseLimitExceeded => ErrorCode.ProfessorCourseLimitExceeded,
            DomainRuleCode.CourseAlreadyAssigned => ErrorCode.CourseAlreadyAssigned,
            DomainRuleCode.TeachingAssignmentNotFound => ErrorCode.TeachingAssignmentNotFound,
            DomainRuleCode.EnrollmentIsNotActive => ErrorCode.EnrollmentInactive,
            DomainRuleCode.EnrollmentMustContainThreeCourses => ErrorCode.EnrollmentMustContainThreeCourses,
            DomainRuleCode.EnrollmentCoursesMustBeDistinct => ErrorCode.EnrollmentCoursesMustBeDistinct,
            DomainRuleCode.EnrollmentCoursesMustBelongToSameProgram => ErrorCode.EnrollmentCoursesMustBelongToSameProgram,
            DomainRuleCode.EnrollmentCoursesMustBeActive => ErrorCode.EnrollmentCoursesMustBeActive,
            DomainRuleCode.EnrollmentCoursesMustHaveDifferentProfessors => ErrorCode.EnrollmentCoursesMustHaveDifferentProfessors,
            _ => ErrorCode.ValidationFailed
        };

        return code switch
        {
            ErrorCode.TeachingAssignmentNotFound => ApplicationResults.NotFound<T>(code),
            ErrorCode.AccountInactive or ErrorCode.CatalogItemInactive or ErrorCode.ProfessorCourseLimitExceeded or
                ErrorCode.CourseAlreadyAssigned or ErrorCode.EnrollmentInactive => ApplicationResults.Conflict<T>(code),
            _ => ApplicationResults.Invalid<T>(code)
        };
    }
}
