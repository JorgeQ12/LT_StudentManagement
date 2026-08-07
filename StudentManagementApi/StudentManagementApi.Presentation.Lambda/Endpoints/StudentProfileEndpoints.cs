using MediatR;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Application.Features.StudentProfiles.DeactivateCurrentStudentAccount;
using StudentManagementApi.Application.Features.StudentProfiles.GetCurrentStudentProfile;
using StudentManagementApi.Application.Features.StudentProfiles.UpdateCurrentStudentProfile;
using StudentManagementApi.Domain;
using StudentManagementApi.Presentation.Lambda.Contracts.StudentProfiles;
using StudentManagementApi.Presentation.Lambda.Results;

namespace StudentManagementApi.Presentation.Lambda.Endpoints;

internal static class StudentProfileEndpoints
{
    public static RouteGroupBuilder MapStudentProfileEndpoints(this RouteGroupBuilder api)
    {
        var group = api.MapGroup("/StudentProfiles")
            .WithTags("Student Profiles")
            .RequireAuthorization(policy => policy.RequireRole(nameof(AccountRole.Student)));

        group.MapGet("/GetCurrentStudentProfile", GetCurrentStudentProfile)
            .Produces<StudentResponse>();
        group.MapPut("/UpdateCurrentStudentProfile", UpdateCurrentStudentProfile)
            .Produces<StudentResponse>()
            .ProducesValidationProblem();
        group.MapDelete("/DeactivateCurrentStudentAccount", DeactivateCurrentStudentAccount)
            .Produces(StatusCodes.Status204NoContent);

        return group;
    }

    private static async Task<IResult> GetCurrentStudentProfile(
        ISender sender,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetCurrentStudentProfileQuery(), cancellationToken);
        return result.ToHttpResult(context);
    }

    private static async Task<IResult> UpdateCurrentStudentProfile(
        UpdateCurrentStudentProfileRequest request,
        ISender sender,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new UpdateCurrentStudentProfileCommand(
            request.FirstName,
            request.LastName,
            request.DateOfBirth,
            request.PhoneNumber,
            request.Email), cancellationToken);
        return result.ToHttpResult(context);
    }

    private static async Task<IResult> DeactivateCurrentStudentAccount(
        ISender sender,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeactivateCurrentStudentAccountCommand(), cancellationToken);
        return result.ToHttpResult(context);
    }
}
