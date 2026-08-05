using Ardalis.Result;
using StudentManagementApi.Application.Common.Errors;

namespace StudentManagementApi.Application.UnitTests;

public sealed class ApplicationResultsTests
{
    public static TheoryData<Result<object>, ResultStatus, ErrorCode> Results => new()
    {
        { ApplicationResults.NotFound<object>(ErrorCode.StudentNotFound), ResultStatus.NotFound, ErrorCode.StudentNotFound },
        { ApplicationResults.Conflict<object>(ErrorCode.EnrollmentAlreadyExists), ResultStatus.Conflict, ErrorCode.EnrollmentAlreadyExists },
        { ApplicationResults.Unauthorized<object>(), ResultStatus.Unauthorized, ErrorCode.Unauthorized },
        { ApplicationResults.Forbidden<object>(), ResultStatus.Forbidden, ErrorCode.Forbidden },
        { ApplicationResults.Error<object>(), ResultStatus.Error, ErrorCode.UnexpectedError },
        { ApplicationResults.Unavailable<object>(), ResultStatus.Unavailable, ErrorCode.ServiceUnavailable }
    };

    [Theory]
    [MemberData(nameof(Results))]
    public void FailureFactoryPreservesStatusAndTechnicalCode(Result<object> result, ResultStatus status, ErrorCode code)
    {
        Assert.Equal(status, result.Status);
        Assert.Equal(code.ToString(), Assert.Single(result.Errors));
    }

    [Fact]
    public void InvalidFactoryPreservesValidationCodeWithoutResponseMessage()
    {
        var result = ApplicationResults.Invalid<object>(ErrorCode.InvalidEmail, "Email");
        var error = Assert.Single(result.ValidationErrors);

        Assert.Equal(ResultStatus.Invalid, result.Status);
        Assert.Equal(nameof(ErrorCode.InvalidEmail), error.ErrorCode);
        Assert.Equal("Email", error.Identifier);
        Assert.Empty(error.ErrorMessage);
    }
}
