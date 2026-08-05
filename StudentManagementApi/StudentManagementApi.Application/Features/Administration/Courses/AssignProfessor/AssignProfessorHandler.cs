using Ardalis.Result;
using MediatR;
using StudentManagementApi.Application.Common.Errors;
using StudentManagementApi.Application.Common.Persistence;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Application.Specifications;
using StudentManagementApi.Domain;
using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Courses;
using StudentManagementApi.Domain.Enrollments;
using StudentManagementApi.Domain.Professors;

namespace StudentManagementApi.Application.Features.Administration.Courses.AssignProfessor;

internal sealed class AssignProfessorHandler(IWriteRepository<Course> courseRepository, IWriteRepository<Professor> professorRepository,
    IWriteRepository<Enrollment> enrollmentRepository, TimeProvider timeProvider) : IRequestHandler<AssignProfessorCommand, Result<CourseResponse>>
{
    public async Task<Result<CourseResponse>> Handle(AssignProfessorCommand request, CancellationToken cancellationToken)
    {
        var courseId = new CourseId(request.CourseId);
        var course = await courseRepository.FirstOrDefaultAsync(new CourseByIdSpec(courseId), cancellationToken);
        if (course is null) return ApplicationResults.NotFound<CourseResponse>(ErrorCode.CourseNotFound);
        if (await enrollmentRepository.AnyAsync(new ActiveEnrollmentUsingCourseSpec(courseId), cancellationToken))
            return ApplicationResults.Conflict<CourseResponse>(ErrorCode.CatalogItemInUse);

        var professorId = new ProfessorId(request.ProfessorId);
        var professor = await professorRepository.FirstOrDefaultAsync(new ProfessorByIdSpec(professorId), cancellationToken);
        if (professor is null) return ApplicationResults.NotFound<CourseResponse>(ErrorCode.ProfessorNotFound);
        if (course.TeachingAssignment?.ProfessorId == professorId) return Result<CourseResponse>.Success(ToResponse(course, professor));

        try
        {
            var previousProfessor = await RemovePreviousAssignmentAsync(course, cancellationToken);
            professor.AssignCourse(TeachingAssignmentId.New(), courseId, course.AcademicProgramId, timeProvider.GetUtcNow());
            if (previousProfessor is not null) await professorRepository.UpdateAsync(previousProfessor, cancellationToken);
            await professorRepository.UpdateAsync(professor, cancellationToken);
            return Result<CourseResponse>.Success(ToResponse(course, professor));
        }
        catch (DomainRuleViolationException exception)
        {
            return DomainErrorMapper.ToResult<CourseResponse>(exception);
        }
    }

    private async Task<Professor?> RemovePreviousAssignmentAsync(Course course, CancellationToken cancellationToken)
    {
        if (course.TeachingAssignment is null) return null;
        var professor = await professorRepository.FirstOrDefaultAsync(
            new ProfessorByIdSpec(course.TeachingAssignment.ProfessorId), cancellationToken);
        if (professor is null) throw new DomainRuleViolationException(DomainRuleCode.TeachingAssignmentNotFound);
        professor.RemoveCourse(course.Id, timeProvider.GetUtcNow());
        return professor;
    }

    private static CourseResponse ToResponse(Course course, Professor professor) =>
        new(course.Id.Value, course.AcademicProgramId.Value, course.Code.Value, course.Name, course.Credits, course.Status,
            professor.Id.Value, professor.FullName);
}
