using Ardalis.Result;
using MediatR;
using StudentManagementApi.Application.Common.Errors;
using StudentManagementApi.Application.Common.Mappings;
using StudentManagementApi.Application.Common.Persistence;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Application.Specifications;
using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Courses;
using StudentManagementApi.Domain.Programs;
using StudentManagementApi.Domain.ValueObjects;

namespace StudentManagementApi.Application.Features.Administration.Courses.CreateCourse;

internal sealed class CreateCourseHandler(IWriteRepository<Course> courseRepository, IWriteRepository<AcademicProgram> programRepository,
    TimeProvider timeProvider) : IRequestHandler<CreateCourseCommand, Result<CourseResponse>>
{
    public async Task<Result<CourseResponse>> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        var programId = new AcademicProgramId(request.AcademicProgramId);
        if (!await programRepository.AnyAsync(new AcademicProgramByIdSpec(programId), cancellationToken))
        {
            return ApplicationResults.NotFound<CourseResponse>(ErrorCode.AcademicProgramNotFound);
        }

        var code = CourseCode.Create(request.Code);
        if (await courseRepository.AnyAsync(new CourseByCodeInProgramSpec(programId, code), cancellationToken))
        {
            return ApplicationResults.Conflict<CourseResponse>(ErrorCode.CourseCodeAlreadyExists);
        }

        try
        {
            var course = Course.Create(CourseId.New(), programId, code, request.Name, timeProvider.GetUtcNow());
            await courseRepository.AddAsync(course, cancellationToken);
            return Result<CourseResponse>.Created(course.ToResponse());
        }
        catch (DomainRuleViolationException exception)
        {
            return DomainErrorMapper.ToResult<CourseResponse>(exception);
        }
    }
}
