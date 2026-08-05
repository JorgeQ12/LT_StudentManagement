using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Application.Features.Authentication.GetAuthenticatedUser;
using StudentManagementApi.Application.Features.Authentication.Login;
using StudentManagementApi.Application.Features.Authentication.Logout;
using StudentManagementApi.Application.Features.Authentication.RegisterStudent;
using StudentManagementApi.Presentation.WebApi.Antiforgery;
using StudentManagementApi.Presentation.WebApi.Contracts.Authentication;
using StudentManagementApi.Presentation.WebApi.RateLimiting;

namespace StudentManagementApi.Presentation.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthenticationController(ISender sender, IAntiforgeryTokenService antiforgeryTokenService) : ControllerBase
{
    /// <summary>Generates an antiforgery token for state-changing requests.</summary>
    /// <returns>The antiforgery token and its header name.</returns>
    /// <response code="200">Returns a new antiforgery token.</response>
    [AllowAnonymous]
    [HttpGet(nameof(GenerateAntiforgeryToken))]
    public AntiforgeryTokenResponse GenerateAntiforgeryToken() => antiforgeryTokenService.Generate(HttpContext);

    /// <summary>Registers a student and starts an authenticated session.</summary>
    /// <param name="request">The student registration information.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The authenticated student details.</returns>
    /// <response code="201">The student was registered successfully.</response>
    /// <response code="400">The registration information is invalid.</response>
    /// <response code="409">The email address or document number is already registered.</response>
    /// <response code="429">The registration rate limit was exceeded.</response>
    [AllowAnonymous]
    [EnableRateLimiting(RateLimitPolicies.Registration)]
    [HttpPost(nameof(RegisterStudent))]
    public Task<Result<AuthenticationSessionResponse>> RegisterStudent(RegisterStudentRequest request, CancellationToken cancellationToken) =>
        sender.Send(new RegisterStudentCommand(request.FirstName, request.LastName, request.DocumentNumber, request.DateOfBirth, request.PhoneNumber,
            request.Email, request.Password), cancellationToken);

    /// <summary>Authenticates an account and starts a session.</summary>
    /// <param name="request">The account credentials.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The authenticated account details.</returns>
    /// <response code="200">The account was authenticated successfully.</response>
    /// <response code="400">The credentials have an invalid format.</response>
    /// <response code="401">The credentials are invalid.</response>
    /// <response code="403">The account is inactive.</response>
    /// <response code="429">The login rate limit was exceeded.</response>
    [AllowAnonymous]
    [EnableRateLimiting(RateLimitPolicies.Login)]
    [HttpPost(nameof(Login))]
    public Task<Result<AuthenticationSessionResponse>> Login(LoginRequest request, CancellationToken cancellationToken) =>
        sender.Send(new LoginCommand(request.Email, request.Password), cancellationToken);

    /// <summary>Ends the current authenticated session.</summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>No content.</returns>
    /// <response code="204">The session was ended successfully.</response>
    /// <response code="401">The request is not authenticated.</response>
    [Authorize]
    [HttpPost(nameof(Logout))]
    public Task<Result<LogoutResponse>> Logout(CancellationToken cancellationToken) => sender.Send(new LogoutCommand(), cancellationToken);

    /// <summary>Gets the currently authenticated account.</summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The authenticated account details.</returns>
    /// <response code="200">Returns the authenticated account.</response>
    /// <response code="401">The request is not authenticated.</response>
    [Authorize]
    [HttpGet(nameof(GetAuthenticatedUser))]
    public Task<Result<AuthenticatedUserResponse>> GetAuthenticatedUser(CancellationToken cancellationToken) =>
        sender.Send(new GetAuthenticatedUserQuery(), cancellationToken);
}
