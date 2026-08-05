namespace StudentManagementApi.Domain;

public enum DomainRuleCode
{
    RequiredValue,
    ValueExceedsMaximumLength,
    InvalidEmail,
    InvalidDocumentNumber,
    InvalidPhoneNumber,
    InvalidDateOfBirth,
    InvalidProgramCode,
    InvalidCourseCode,
    AccountIsInactive,
    CatalogItemIsInactive,
    ProfessorCourseLimitExceeded,
    CourseAlreadyAssigned,
    TeachingAssignmentNotFound,
    EnrollmentIsNotActive,
    EnrollmentMustContainThreeCourses,
    EnrollmentCoursesMustBeDistinct,
    EnrollmentCoursesMustBelongToSameProgram,
    EnrollmentCoursesMustBeActive,
    EnrollmentCoursesMustHaveDifferentProfessors
}
