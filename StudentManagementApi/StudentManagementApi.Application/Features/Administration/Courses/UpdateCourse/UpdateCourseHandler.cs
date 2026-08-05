using Ardalis.Result;
using MediatR;
using StudentManagementApi.Application.Common.Errors;
using StudentManagementApi.Application.Common.Mappings;
using StudentManagementApi.Application.Common.Persistence;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Application.Specifications;
using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Courses;
using StudentManagementApi.Domain.Enrollments;
using StudentManagementApi.Domain.ValueObjects;

namespace StudentManagementApi.Application.Features.Administration.Courses.UpdateCourse;

internal sealed class UpdateCourseHandler(IWriteRepository<Course> courseRepository, IWriteRepository<Enrollment> enrollmentRepository,
    TimeProvider timeProvider) : IRequestHandler<UpdateCourseCommand, Result<CourseResponse>>
{
    public async Task<Result<CourseResponse>> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
    {
        var courseId = new CourseId(request.CourseId);
        var course = await courseRepository.FirstOrDefaultAsync(new CourseByIdSpec(courseId), cancellationToken);
        if (course is null) return ApplicationResults.NotFound<CourseResponse>(ErrorCode.CourseNotFound);
        if (await enrollmentRepository.AnyAsync(new ActiveEnrollmentUsingCourseSpec(courseId), cancellationToken))
            return ApplicationResults.Conflict<CourseResponse>(ErrorCode.CatalogItemInUse);

        var code = CourseCode.Create(request.Code);
        var duplicate = await courseRepository.FirstOrDefaultAsync(new CourseByCodeInProgramSpec(course.AcademicProgramId, code), cancellationToken);
        if (duplicate is not null && duplicate.Id != courseId) return ApplicationResults.Conflict<CourseResponse>(ErrorCode.CourseCodeAlreadyExists);

        try
        {
            course.Update(code, request.Name, timeProvider.GetUtcNow());
            await courseRepository.UpdateAsync(course, cancellationToken);
            return Result<CourseResponse>.Success(course.ToResponse());
        }
        catch (DomainRuleViolationException exception)
        {
            return DomainErrorMapper.ToResult<CourseResponse>(exception);
        }
    }
}
