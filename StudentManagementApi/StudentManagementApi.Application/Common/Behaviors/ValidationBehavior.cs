using Ardalis.Result;
using FluentValidation;
using MediatR;
using StudentManagementApi.Application.Common.Errors;

namespace StudentManagementApi.Application.Common.Behaviors;

internal sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!validators.Any())
        {
            return await next(cancellationToken);
        }

        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(validators.Select(validator => validator.ValidateAsync(context, cancellationToken)));

        var errors = validationResults
            .SelectMany(result => result.Errors)
            .Where(failure => failure is not null)
            .Select(failure => new ValidationError(
                failure.PropertyName,
                string.Empty,
                string.IsNullOrWhiteSpace(failure.ErrorCode) ? ErrorCode.ValidationFailed.ToString() : failure.ErrorCode,
                ValidationSeverity.Error
            ))
            .ToArray();

        return errors.Length == 0 ? await next(cancellationToken) : InvalidResultFactory.Create<TResponse>(errors);
    }
}
