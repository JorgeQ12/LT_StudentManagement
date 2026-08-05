using Ardalis.Result;
using MediatR;

namespace StudentManagementApi.Application.Common.Messaging;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>, ICommandMarker;
