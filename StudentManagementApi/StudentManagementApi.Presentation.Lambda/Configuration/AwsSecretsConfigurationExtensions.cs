using System.Text.Json;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;

namespace StudentManagementApi.Presentation.Lambda.Configuration;

internal static class AwsSecretsConfigurationExtensions
{
    private const string ApplicationSecretArnVariable = "APPLICATION_SECRET_ARN";

    public static async Task AddApplicationSecretFromAwsAsync(this ConfigurationManager configuration)
    {
        var secretArn = Environment.GetEnvironmentVariable(ApplicationSecretArnVariable);
        if (string.IsNullOrWhiteSpace(secretArn)) return;

        using var client = new AmazonSecretsManagerClient();
        var response = await client.GetSecretValueAsync(new GetSecretValueRequest { SecretId = secretArn });
        if (string.IsNullOrWhiteSpace(response.SecretString))
            throw new InvalidOperationException($"AWS secret '{secretArn}' is empty.");

        using var document = JsonDocument.Parse(response.SecretString);
        var root = document.RootElement;
        var values = new Dictionary<string, string?>
        {
            ["ConnectionStrings:StudentManagementDb"] = root.GetProperty("ConnectionString").GetString(),
            ["Security:Jwt:SigningKey"] = root.GetProperty("JwtSigningKey").GetString()
        };

        configuration.AddInMemoryCollection(values);
    }
}
