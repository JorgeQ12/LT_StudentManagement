namespace StudentManagementApi.Infrastructure.Persistence.SqlServer.Bootstrap;

public sealed class AdministratorBootstrapOptions
{
    public const string SectionName = "BootstrapAdministrator";

    public string? Email { get; init; }
    public string? Password { get; init; }

    public bool IsConfigured => !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Password);
}
