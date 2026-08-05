using Ardalis.Result;

namespace StudentManagementApi.Application.Common.Behaviors;

internal static class InvalidResultFactory
{
    public static TResponse Create<TResponse>(IEnumerable<ValidationError> errors)
    {
        var responseType = typeof(TResponse);
        if (!responseType.IsGenericType || responseType.GetGenericTypeDefinition() != typeof(Result<>))
        {
            throw new InvalidOperationException(responseType.FullName);
        }

        var method =
            responseType.GetMethod(nameof(Result<object>.Invalid), [typeof(IEnumerable<ValidationError>)])
            ?? throw new InvalidOperationException(responseType.FullName);

        return (TResponse)(method.Invoke(null, [errors]) ?? throw new InvalidOperationException(responseType.FullName));
    }
}
