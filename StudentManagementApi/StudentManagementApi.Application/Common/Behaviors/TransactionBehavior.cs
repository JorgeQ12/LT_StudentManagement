using MediatR;
using StudentManagementApi.Application.Common.Messaging;
using StudentManagementApi.Application.Common.Persistence;

namespace StudentManagementApi.Application.Common.Behaviors;

internal sealed class TransactionBehavior<TRequest, TResponse>(ITransactionManager transactionManager)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken) =>
        request is ICommandMarker
            ? transactionManager.ExecuteAsync<TResponse>(token => next(token), cancellationToken)
            : next(cancellationToken);
}
