using System.Text.Json.Serialization;

namespace StudentManagementApi.Application.Contracts;

public sealed record AuthenticationSessionResponse(
    AuthenticatedUserResponse User,
    [property: JsonIgnore] string AccessToken,
    [property: JsonIgnore] DateTimeOffset ExpiresAtUtc);
