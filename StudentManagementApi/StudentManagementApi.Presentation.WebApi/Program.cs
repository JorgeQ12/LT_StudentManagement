using StudentManagementApi.Presentation.WebApi.Configuration;
using StudentManagementApi.Presentation.WebApi;

EnvironmentFileLoader.Load();
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddWebApi(builder.Configuration, builder.Environment);

var app = builder.Build();
await app.UseWebApiAsync();
await app.RunAsync();

public partial class Program;
