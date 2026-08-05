namespace StudentManagementApi.Application.Common.Errors;

public interface IErrorMessageProvider
{
    ErrorMessage Resolve(ErrorCode code);
}
