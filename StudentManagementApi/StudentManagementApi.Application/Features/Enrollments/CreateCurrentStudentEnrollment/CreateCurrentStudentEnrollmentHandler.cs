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

namespace StudentManagementApi.Application.Features.Enrollments.CreateCurrentStudentEnrollment;

internal sealed class CreateCurrentStudentEnrollmentHandler(
    ICurrentUser currentUser,
    IWriteRepository<AcademicProgram> academicProgramRepository,
    IWriteRepository<Course> courseRepository,
    IWriteRepository<Enrollment> enrollmentRepository,
    TimeProvider timeProvider
) : IRequestHandler<CreateCurrentStudentEnrollmentCommand, Result<EnrollmentResponse>>
{
    public async Task<Result<EnrollmentResponse>> Handle(CreateCurrentStudentEnrollmentCommand request, CancellationToken cancellationToken)
    {
        if (currentUser.StudentId is null)
        {
            return ApplicationResults.Forbidden<EnrollmentResponse>();
        }

        var studentId = currentUser.StudentId.Value;
        if (await enrollmentRepository.AnyAsync(new ActiveEnrollmentByStudentSpec(studentId), cancellationToken))
        {
            return ApplicationResults.Conflict<EnrollmentResponse>(ErrorCode.EnrollmentAlreadyExists);
        }

        var academicProgramId = new AcademicProgramId(request.AcademicProgramId);
        var academicProgram = await academicProgramRepository.FirstOrDefaultAsync(new AcademicProgramByIdSpec(academicProgramId), cancellationToken);

        if (academicProgram is null)
        {
            return ApplicationResults.NotFound<EnrollmentResponse>(ErrorCode.AcademicProgramNotFound);
        }

        var courseIds = request.CourseIds.Select(id => new CourseId(id)).ToArray();
        var courses = await courseRepository.ListAsync(new CoursesByIdsSpec(courseIds), cancellationToken);

        if (courses.Count != Enrollment.RequiredCourseCount)
        {
            return ApplicationResults.NotFound<EnrollmentResponse>(ErrorCode.CourseNotFound);
        }

        try
        {
            var enrollment = Enrollment.Create(EnrollmentId.New(), studentId, academicProgramId, courses.Select(ToSelection).ToArray(),
                timeProvider.GetUtcNow());

            await enrollmentRepository.AddAsync(enrollment, cancellationToken);
            return Result<EnrollmentResponse>.Created(enrollment.ToResponse(academicProgram, courses));
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
