using Amazon.Lambda.AspNetCoreServer.Hosting;
using Microsoft.EntityFrameworkCore;
using StudentManagementApi.Infrastructure.Persistence.SqlServer.Bootstrap;
using StudentManagementApi.Infrastructure.Persistence.SqlServer.Context;
using StudentManagementApi.Presentation.Lambda;
using StudentManagementApi.Presentation.Lambda.Configuration;

var builder = WebApplication.CreateBuilder(args);

await builder.Configuration.AddApplicationSecretFromAwsAsync();
builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);
builder.Services.AddLambdaPresentation(builder.Configuration, builder.Environment);

var app = builder.Build();
app.UseLambdaPresentation();

await using (var scope = app.Services.CreateAsyncScope())
{
    if (builder.Configuration.GetValue<bool>("DatabaseInitialization:ApplyMigrations"))
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<StudentManagementWriteDbContext>();
        await dbContext.Database.MigrateAsync();
    }

    await scope.ServiceProvider.GetRequiredService<AdministratorBootstrapper>().CreateIfConfiguredAsync();
}

app.Run();

public partial class Program;
