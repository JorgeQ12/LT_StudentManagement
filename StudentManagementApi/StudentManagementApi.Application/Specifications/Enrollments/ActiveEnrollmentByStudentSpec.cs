using Ardalis.Specification;
using StudentManagementApi.Domain;
using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Enrollments;

namespace StudentManagementApi.Application.Specifications;

internal sealed class ActiveEnrollmentByStudentSpec : SingleResultSpecification<Enrollment>
{
    public ActiveEnrollmentByStudentSpec(StudentId studentId) =>
        Query
            .Where(enrollment => enrollment.StudentId == studentId && enrollment.Status == EnrollmentStatus.Active)
            .Include(enrollment => enrollment.AcademicProgram)
            .Include(enrollment => enrollment.Student)
                .ThenInclude(student => student.Account)
            .Include(enrollment => enrollment.Courses)
                .ThenInclude(enrollmentCourse => enrollmentCourse.Course)
                    .ThenInclude(course => course.TeachingAssignment!)
                        .ThenInclude(assignment => assignment.Professor);
}
