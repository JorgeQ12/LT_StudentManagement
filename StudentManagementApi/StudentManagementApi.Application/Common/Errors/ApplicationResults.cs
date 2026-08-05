using Ardalis.Result;

namespace StudentManagementApi.Application.Common.Errors;

public static class ApplicationResults
{
    public static Result<T> Invalid<T>(ErrorCode code, string identifier = "") =>
        Result<T>.Invalid(new ValidationError(identifier, string.Empty, code.ToString(), ValidationSeverity.Error));

    public static Result<T> Invalid<T>(IEnumerable<ValidationError> errors) => Result<T>.Invalid(errors);

    public static Result<T> NotFound<T>(ErrorCode code) => Result<T>.NotFound(code.ToString());

    public static Result<T> Conflict<T>(ErrorCode code) => Result<T>.Conflict(code.ToString());

    public static Result<T> Unauthorized<T>(ErrorCode code = ErrorCode.Unauthorized) => Result<T>.Unauthorized(code.ToString());

    public static Result<T> Forbidden<T>(ErrorCode code = ErrorCode.Forbidden) => Result<T>.Forbidden(code.ToString());

    public static Result<T> Error<T>(ErrorCode code = ErrorCode.UnexpectedError) => Result<T>.Error(code.ToString());

    public static Result<T> Unavailable<T>(ErrorCode code = ErrorCode.ServiceUnavailable) => Result<T>.Unavailable(code.ToString());
}
