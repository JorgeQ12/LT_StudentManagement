using MediatR;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Application.Features.Administration.Students.ActivateStudent;
using StudentManagementApi.Application.Features.Administration.Students.CreateStudent;
using StudentManagementApi.Application.Features.Administration.Students.DeactivateStudent;
using StudentManagementApi.Application.Features.Administration.Students.GetAllStudents;
using StudentManagementApi.Application.Features.Administration.Students.GetStudentById;
using StudentManagementApi.Application.Features.Administration.Students.UpdateStudent;
using StudentManagementApi.Domain;
using StudentManagementApi.Presentation.Lambda.Contracts.Administration.Students;
using StudentManagementApi.Presentation.Lambda.Results;

namespace StudentManagementApi.Presentation.Lambda.Endpoints;

internal static class AdministrationStudentEndpoints
{
    public static RouteGroupBuilder MapAdministrationStudentEndpoints(this RouteGroupBuilder api)
    {
        var group = api.MapGroup("/AdministrationStudents")
            .WithTags("Administration - Students")
            .RequireAuthorization(policy => policy.RequireRole(nameof(AccountRole.Administrator)));

        group.MapGet("/GetAllStudents", GetAllStudents)
            .Produces<PagedResponse<StudentResponse>>();
        group.MapGet("/GetStudentById", GetStudentById)
            .Produces<StudentResponse>();
        group.MapPost("/CreateStudent", CreateStudent)
            .Produces<StudentResponse>(StatusCodes.Status201Created)
            .ProducesValidationProblem();
        group.MapPut("/UpdateStudent", UpdateStudent)
            .Produces<StudentResponse>()
            .ProducesValidationProblem();
        group.MapPut("/ActivateStudent", ActivateStudent)
            .Produces<StudentResponse>();
        group.MapDelete("/DeactivateStudent", DeactivateStudent)
            .Produces(StatusCodes.Status204NoContent);

        return group;
    }

    private static async Task<IResult> GetAllStudents(
        ISender sender,
        HttpContext context,
        CancellationToken cancellationToken,
        int pageNumber = 1,
        int pageSize = 20,
        string? search = null,
        AccountStatus? status = null)
    {
        var result = await sender.Send(
            new GetAllStudentsQuery(pageNumber, pageSize, search, status),
            cancellationToken);
        return result.ToHttpResult(context);
    }

    private static async Task<IResult> GetStudentById(
        Guid studentId,
        ISender sender,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetStudentByIdQuery(studentId), cancellationToken);
        return result.ToHttpResult(context);
    }

    private static async Task<IResult> CreateStudent(
        CreateStudentRequest request,
        ISender sender,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new CreateStudentCommand(
            request.FirstName,
            request.LastName,
            request.DocumentNumber,
            request.DateOfBirth,
            request.PhoneNumber,
            request.Email,
            request.Password), cancellationToken);
        return result.ToHttpResult(context);
    }

    private static async Task<IResult> UpdateStudent(
        UpdateStudentRequest request,
        ISender sender,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new UpdateStudentCommand(
            request.StudentId,
            request.FirstName,
            request.LastName,
            request.DocumentNumber,
            request.DateOfBirth,
            request.PhoneNumber,
            request.Email), cancellationToken);
        return result.ToHttpResult(context);
    }

    private static async Task<IResult> ActivateStudent(
        ActivateStudentRequest request,
        ISender sender,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new ActivateStudentCommand(request.StudentId), cancellationToken);
        return result.ToHttpResult(context);
    }

    private static async Task<IResult> DeactivateStudent(
        Guid studentId,
        ISender sender,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeactivateStudentCommand(studentId), cancellationToken);
        return result.ToHttpResult(context);
    }
}
