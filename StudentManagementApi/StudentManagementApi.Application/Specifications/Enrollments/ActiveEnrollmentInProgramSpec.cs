using Ardalis.Specification;
using StudentManagementApi.Domain;
using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Enrollments;

namespace StudentManagementApi.Application.Specifications;

internal sealed class ActiveEnrollmentInProgramSpec : SingleResultSpecification<Enrollment>
{
    public ActiveEnrollmentInProgramSpec(AcademicProgramId academicProgramId) =>
        Query.Where(enrollment => enrollment.AcademicProgramId == academicProgramId && enrollment.Status == EnrollmentStatus.Active);
}
