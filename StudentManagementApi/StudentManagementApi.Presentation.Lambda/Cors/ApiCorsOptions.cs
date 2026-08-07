namespace StudentManagementApi.Presentation.Lambda.Cors;

public sealed class ApiCorsOptions
{
    public const string SectionName = "Cors";

    public bool AllowLocalhost { get; init; }
    public string[] AllowedOrigins { get; init; } = [];
}
