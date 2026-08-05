using Ardalis.Result;
using MediatR;
using StudentManagementApi.Application.Contracts;

namespace StudentManagementApi.Application.Features.Authentication.Logout;

internal sealed class LogoutHandler : IRequestHandler<LogoutCommand, Result<LogoutResponse>>
{
    public Task<Result<LogoutResponse>> Handle(LogoutCommand request, CancellationToken cancellationToken) =>
        Task.FromResult(Result<LogoutResponse>.NoContent());
}
