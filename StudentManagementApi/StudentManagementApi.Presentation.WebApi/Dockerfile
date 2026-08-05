FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY . .
RUN dotnet restore "StudentManagementApi.Presentation.WebApi/StudentManagementApi.Presentation.WebApi.csproj"
RUN dotnet publish "StudentManagementApi.Presentation.WebApi/StudentManagementApi.Presentation.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish --no-restore /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
USER $APP_UID
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "StudentManagementApi.Presentation.WebApi.dll"]
