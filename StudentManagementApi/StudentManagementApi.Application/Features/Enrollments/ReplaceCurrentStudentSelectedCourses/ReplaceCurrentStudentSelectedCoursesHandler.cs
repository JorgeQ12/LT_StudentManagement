using Ardalis.Result;
using MediatR;
using StudentManagementApi.Application.Common.Errors;
using StudentManagementApi.Application.Common.Mappings;
using StudentManagementApi.Application.Common.Persistence;
using StudentManagementApi.Application.Common.Security;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Application.Specifications;
using StudentManagementApi.Domain;
using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Courses;
using StudentManagementApi.Domain.Enrollments;
using StudentManagementApi.Domain.Programs;

namespace StudentManagementApi.Application.Features.Enrollments.ReplaceCurrentStudentSelectedCourses;

internal sealed class ReplaceCurrentStudentSelectedCoursesHandler(
    ICurrentUser currentUser,
    IWriteRepository<Enrollment> enrollmentRepository,
    IWriteRepository<Course> courseRepository,
    IWriteRepository<AcademicProgram> academicProgramRepository,
    TimeProvider timeProvider
) : IRequestHandler<ReplaceCurrentStudentSelectedCoursesCommand, Result<EnrollmentResponse>>
{
    public async Task<Result<EnrollmentResponse>> Handle(ReplaceCurrentStudentSelectedCoursesCommand request, CancellationToken cancellationToken)
    {
        if (currentUser.StudentId is null)
        {
            return ApplicationResults.Forbidden<EnrollmentResponse>();
        }

        var enrollment = await enrollmentRepository.FirstOrDefaultAsync(new ActiveEnrollmentByStudentSpec(currentUser.StudentId.Value), cancellationToken);

        if (enrollment is null)
        {
            return ApplicationResults.NotFound<EnrollmentResponse>(ErrorCode.EnrollmentNotFound);
        }

        var courseIds = request.CourseIds.Select(id => new CourseId(id)).ToArray();
        var courses = await courseRepository.ListAsync(new CoursesByIdsSpec(courseIds), cancellationToken);

        if (courses.Count != Enrollment.RequiredCourseCount)
        {
            return ApplicationResults.NotFound<EnrollmentResponse>(ErrorCode.CourseNotFound);
        }

        var academicProgram = await academicProgramRepository.FirstOrDefaultAsync(new AcademicProgramByIdSpec(enrollment.AcademicProgramId), cancellationToken);

        if (academicProgram is null)
        {
            return ApplicationResults.NotFound<EnrollmentResponse>(ErrorCode.AcademicProgramNotFound);
        }

        try
        {
            enrollment.ReplaceSelectedCourses(courses.Select(ToSelection).ToArray(), timeProvider.GetUtcNow());
            await enrollmentRepository.UpdateAsync(enrollment, cancellationToken);
            return Result<EnrollmentResponse>.Success(enrollment.ToResponse(academicProgram, courses));
        }
        catch (DomainRuleViolationException exception)
        {
            return DomainErrorMapper.ToResult<EnrollmentResponse>(exception);
        }
    }

    private static CourseSelection ToSelection(Course course)
    {
        if (course.TeachingAssignment is null)
        {
            throw new DomainRuleViolationException(DomainRuleCode.EnrollmentCoursesMustBeActive);
        }

        return new CourseSelection(
            course.Id,
            course.AcademicProgramId,
            course.TeachingAssignment.ProfessorId,
            course.Status,
            course.TeachingAssignment.Professor.Status,
            course.Credits
        );
    }
}
