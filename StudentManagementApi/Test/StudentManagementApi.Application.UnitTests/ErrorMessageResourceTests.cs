using StudentManagementApi.Application.Common.Errors;

namespace StudentManagementApi.Application.UnitTests;

public sealed class ErrorMessageResourceTests
{
    [Fact]
    public void EveryErrorCodeHasLocalizedTitleAndDetail()
    {
        var provider = new ResourceErrorMessageProvider();

        foreach (var code in Enum.GetValues<ErrorCode>())
        {
            var message = provider.Resolve(code);
            Assert.False(string.IsNullOrWhiteSpace(message.Title), code.ToString());
            Assert.False(string.IsNullOrWhiteSpace(message.Detail), code.ToString());
            Assert.NotEqual(code.ToString(), message.Title);
            Assert.NotEqual(code.ToString(), message.Detail);
        }
    }
}
