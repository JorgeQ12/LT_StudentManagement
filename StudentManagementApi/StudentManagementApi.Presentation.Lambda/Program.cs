using Amazon.Lambda.AspNetCoreServer.Hosting;
using StudentManagementApi.Presentation.Lambda;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);
builder.Services.AddLambdaPresentation(builder.Configuration, builder.Environment);

var app = builder.Build();
app.UseLambdaPresentation();
app.Run();

public partial class Program;
