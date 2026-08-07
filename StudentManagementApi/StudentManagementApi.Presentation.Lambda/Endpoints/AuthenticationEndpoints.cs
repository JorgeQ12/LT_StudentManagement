using MediatR;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Application.Features.Authentication.GetAuthenticatedUser;
using StudentManagementApi.Application.Features.Authentication.Login;
using StudentManagementApi.Application.Features.Authentication.Logout;
using StudentManagementApi.Application.Features.Authentication.RegisterStudent;
using StudentManagementApi.Presentation.Lambda.Antiforgery;
using StudentManagementApi.Presentation.Lambda.Contracts.Authentication;
using StudentManagementApi.Presentation.Lambda.RateLimiting;
using StudentManagementApi.Presentation.Lambda.Results;

namespace StudentManagementApi.Presentation.Lambda.Endpoints;

internal static class AuthenticationEndpoints
{
    public static RouteGroupBuilder MapAuthenticationEndpoints(this RouteGroupBuilder api)
    {
        var group = api.MapGroup("/Authentication").WithTags("Authentication");

        group.MapGet("/GenerateAntiforgeryToken", (HttpContext context, IAntiforgeryTokenService tokens) =>
                Microsoft.AspNetCore.Http.Results.Ok(tokens.Generate(context)))
            .Produces<AntiforgeryTokenResponse>()
            .AllowAnonymous();
        group.MapPost("/RegisterStudent", RegisterStudent)
            .Produces<AuthenticatedUserResponse>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .AllowAnonymous()
            .RequireRateLimiting(RateLimitPolicies.Registration);
        group.MapPost("/Login", Login)
            .Produces<AuthenticatedUserResponse>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .AllowAnonymous()
            .RequireRateLimiting(RateLimitPolicies.Login);
        group.MapPost("/Logout", Logout)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .RequireAuthorization();
        group.MapGet("/GetAuthenticatedUser", GetAuthenticatedUser)
            .Produces<AuthenticatedUserResponse>()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .RequireAuthorization();

        return group;
    }

    private static async Task<IResult> RegisterStudent(
        RegisterStudentRequest request,
        ISender sender,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new RegisterStudentCommand(
            request.FirstName,
            request.LastName,
            request.DocumentNumber,
            request.DateOfBirth,
            request.PhoneNumber,
            request.Email,
            request.Password), cancellationToken);
        return result.ToHttpResult(context);
    }

    private static async Task<IResult> Login(
        LoginRequest request,
        ISender sender,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new LoginCommand(request.Email, request.Password), cancellationToken);
        return result.ToHttpResult(context);
    }

    private static async Task<IResult> Logout(ISender sender, HttpContext context, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new LogoutCommand(), cancellationToken);
        return result.ToHttpResult(context);
    }

    private static async Task<IResult> GetAuthenticatedUser(
        ISender sender,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetAuthenticatedUserQuery(), cancellationToken);
        return result.ToHttpResult(context);
    }
}
