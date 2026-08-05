using System.Globalization;
using System.Resources;

namespace StudentManagementApi.Application.Common.Errors;

internal sealed class ResourceErrorMessageProvider : IErrorMessageProvider
{
    private static readonly CultureInfo Culture = CultureInfo.GetCultureInfo("es-CO");
    private static readonly ResourceManager ResourceManager = new(
        "StudentManagementApi.Application.Resources.ErrorMessages",
        typeof(ResourceErrorMessageProvider).Assembly);

    public ErrorMessage Resolve(ErrorCode code) => new(
        ResourceManager.GetString($"{code}_Title", Culture) ?? code.ToString(),
        ResourceManager.GetString($"{code}_Detail", Culture) ?? code.ToString());
}
