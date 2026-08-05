using Ardalis.Specification;
using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Courses;
using StudentManagementApi.Domain.ValueObjects;

namespace StudentManagementApi.Application.Specifications;

internal sealed class CourseByCodeInProgramSpec : SingleResultSpecification<Course>
{
    public CourseByCodeInProgramSpec(AcademicProgramId academicProgramId, CourseCode code) =>
        Query.Where(course => course.AcademicProgramId == academicProgramId && course.Code == code);
}
