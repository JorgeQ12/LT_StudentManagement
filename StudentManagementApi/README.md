# Student Management API

Student Management API is a .NET 10 and SQL Server backend for academic registration. It applies hexagonal architecture, domain-driven design, CQRS with MediatR, Ardalis Specification, Ardalis Result, EF Core Code First, typed error codes, JWT authentication, and CSRF protection.

## Business rules

- A student can have only one active enrollment.
- An enrollment contains exactly three distinct courses from one academic program.
- Every course is worth three credits, so an enrollment totals nine credits.
- The three selected courses must have three different professors.
- A professor can teach at most two courses within an academic program.
- Courses referenced by active enrollments cannot be modified, reassigned, or deactivated.
- Delete operations deactivate or cancel records; they never physically delete business data.
- Students can see only the names of classmates who share each selected course.

## Architecture

The dependency direction is inward:

```text
Presentation.Lambda --> Application <-- Infrastructure.Persistence.SqlServer
         │                  │          <── Infrastructure.Security
         └──────────────────┴─────────────> Domain
```

- `Domain` contains aggregates, entities, value objects, identifiers, enums, and invariants. It has no framework dependency.
- `Application` contains use cases organized vertically, repository ports, specifications, validators, Result factories, and security abstractions.
- `Infrastructure.Persistence.SqlServer` implements EF Core contexts, repositories, configurations, migrations, transactions, concurrency, and catalog seeding.
- `Infrastructure.Security` implements password hashing, JWT creation and validation, current-user access, and authentication cookies.
- `Presentation.Lambda` contains grouped Minimal API endpoints, AWS Lambda hosting, Problem Details translation, antiforgery enforcement, and rate limiting.
- Unit tests are separated into domain and application projects.

Commands inject only `IWriteRepository<TAggregate>` and run inside serializable transactions. Queries inject only `IReadRepository<TAggregate>` and use a dedicated no-tracking context. Neither `IQueryable`, `DbSet`, nor EF Core types leave Persistence.

## Error contract

Expected validation and business failures return `Ardalis.Result` with a stable English `ErrorCode`. Unexpected exceptions and EF concurrency exceptions are handled globally. Presentation translates both paths to RFC Problem Details and never exposes exception messages, SQL, stack traces, password hashes, signing keys, or JWT values.

```json
{
  "title": "Estudiante no encontrado",
  "status": 404,
  "detail": "No se encontró el estudiante solicitado.",
  "code": "StudentNotFound",
  "traceId": "0H..."
}
```

All visible error text is stored in `ErrorMessages.es-CO.resx`. Handlers and endpoints contain no response-message literals. Successful responses return a resource or `204 No Content` without decorative messages.

## Configuration

The Lambda uses the standard .NET configuration providers. The root `.env.example` documents the required environment variables; export equivalent values locally or configure them in Lambda before starting the API:

```powershell
dotnet run --project StudentManagementApi.Presentation.Lambda
```

The API never applies migrations or inserts academic catalog data automatically; database migrations are an explicit operator action. Deployment secrets must be injected through the AWS environment or its secret-management services.

Development CORS accepts HTTP and HTTPS loopback origins on any port. Browser clients must send requests with credentials enabled because authentication and antiforgery use cookies. Non-development environments accept only origins configured through indexed `Cors__AllowedOrigins__N` variables.

Swagger UI is available at `/swagger`, and its OpenAPI document is exposed at `/openapi/v1.json`.

Development infrastructure and deployment are managed with Terraform. See [`docs/terraform-development.md`](docs/terraform-development.md) for the one-time AWS and GitHub configuration.

## Authentication and CSRF flow

1. Call `GET /api/Authentication/GenerateAntiforgeryToken`.
2. Keep the `__Host-student_csrf` cookie and send the returned token in `X-CSRF-TOKEN` for every unsafe request.
3. Register or log in. The API returns user data and writes the 15-minute JWT to `__Host-student_access_token`; the token is never present in the JSON response.
4. Generate a new antiforgery request token after authentication changes, because the token is bound to the current identity.
5. Send cookies with credentials enabled and include the antiforgery header for POST, PUT, PATCH, and DELETE requests.

Both cookies are Secure, SameSite Strict, use Path `/`, and use the `__Host-` prefix. The access-token cookie is HttpOnly. JWT claims are `sub`, `role`, `student_id` for students, and `jti`.

## Tests

Run the unit tests without external infrastructure:

```powershell
dotnet test StudentManagementApi.slnx
```

The unit suites cover value objects, domain invariants, aggregate state transitions, localized error resources, Result statuses, domain-error mappings, and in-memory specification filtering, ordering, and projections.

## Database migrations

Generate and apply migrations using the configured connection string:

```powershell
dotnet ef migrations add MigrationName --project StudentManagementApi.Infrastructure.Persistence.SqlServer --startup-project StudentManagementApi.Infrastructure.Persistence.SqlServer --context StudentManagementDbContext
dotnet ef database update --project StudentManagementApi.Infrastructure.Persistence.SqlServer --startup-project StudentManagementApi.Infrastructure.Persistence.SqlServer --context StudentManagementDbContext
```

Enums are stored as readable text. The schema uses unique indexes for email, document numbers, codes, and teaching assignments; a filtered unique index enforces one active enrollment per student; `rowversion` provides optimistic concurrency; and foreign keys restrict destructive deletes.
