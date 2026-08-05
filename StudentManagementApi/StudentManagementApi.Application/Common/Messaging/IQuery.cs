using Ardalis.Result;
using MediatR;

namespace StudentManagementApi.Application.Common.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
