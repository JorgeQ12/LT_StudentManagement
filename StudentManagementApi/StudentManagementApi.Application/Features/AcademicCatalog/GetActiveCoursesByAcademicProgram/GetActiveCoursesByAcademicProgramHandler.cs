using Ardalis.Result;
using MediatR;
using StudentManagementApi.Application.Common.Errors;
using StudentManagementApi.Application.Common.Persistence;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Application.Specifications;
using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Courses;
using StudentManagementApi.Domain.Programs;

namespace StudentManagementApi.Application.Features.AcademicCatalog.GetActiveCoursesByAcademicProgram;

internal sealed class GetActiveCoursesByAcademicProgramHandler(
    IReadRepository<AcademicProgram> academicProgramRepository,
    IReadRepository<Course> courseRepository
) : IRequestHandler<GetActiveCoursesByAcademicProgramQuery, Result<IReadOnlyCollection<CourseResponse>>>
{
    public async Task<Result<IReadOnlyCollection<CourseResponse>>> Handle(GetActiveCoursesByAcademicProgramQuery request, CancellationToken cancellationToken)
    {
        var academicProgramId = new AcademicProgramId(request.AcademicProgramId);
        var academicProgramExists = await academicProgramRepository.AnyAsync(
            new AcademicProgramByIdSpec(academicProgramId),
            cancellationToken);

        if (!academicProgramExists)
        {
            return ApplicationResults.NotFound<IReadOnlyCollection<CourseResponse>>(ErrorCode.AcademicProgramNotFound);
        }

        var courses = await courseRepository.ListAsync(new ActiveCoursesByProgramSpec(academicProgramId), cancellationToken);

        return Result<IReadOnlyCollection<CourseResponse>>.Success(courses);
    }
}
