# Student Management Web

Angular 22 frontend for **Portal Académico**. It is a client-rendered, zoneless SPA built with standalone components, strict TypeScript, Signal Forms, Signals, RxJS, Tailwind CSS v4, Angular CDK and Lucide icons.

The interface targets Spanish-speaking users in Colombia. Source code and technical documentation use English.

## Architecture

The application uses a feature-first structure:

```text
src/app/
|-- core/                         # Session, HTTP, guards and the application shell
|-- features/
|   `-- <feature>/
|       |-- pages/                # Routable components (external TS, HTML and CSS)
|       |-- components/           # UI owned by the feature
|       |-- services/             # API-only services: transport and HTTP
|       |-- facades/              # UI state, orchestration and use cases
|       |-- models/               # Presentation models and pure rules
|       |-- public-api.ts         # Public boundary for cross-feature access
|       `-- <feature>.routes.ts   # Lazy routes and feature providers
`-- shared/ui/                    # Reusable presentation primitives
```

Pages act as route containers and compose smaller feature components. They inject facades, never API services directly. Facades orchestrate use cases and expose readonly Signals. API services inject `HttpClient`, map transport DTOs and perform no UI orchestration.

Cross-feature imports go through a feature public API. ESLint prevents deep imports across feature boundaries.

## Runtime and security

- `API_BASE_URL` defaults to `/api` so browser traffic remains same-origin.
- Every HTTP request uses credentials.
- JWTs and antiforgery tokens are never stored in browser storage.
- The session store keeps only the current user, role and session status in memory.
- Unsafe requests receive an in-memory `X-CSRF-TOKEN` automatically.
- Authentication and role guards improve navigation, while the API remains the authorization authority.
- `401` responses clear the client session and redirect to login with a session-expired notice.

## Local development

Requirements:

- Node.js 24
- npm 11
- Backend listening on `https://localhost:44325`

```bash
npm ci
npm start
```

The development server runs at `https://localhost:4200`. HTTPS is required because authentication and antiforgery cookies use the `Secure` and `__Host-` protections. Angular replaces `environment.ts` with `environment.development.ts`, while `proxy.conf.json` forwards the same-origin `/api` path to `https://localhost:44325`.

## Verification

```bash
npm run lint
npm run format:check
npm run typecheck
npm run build
```

`npm run verify` runs linting, formatting, type checking and the production build.

### OpenAPI contract check

Start the backend in Development and run:

```bash
npm run test:contract
```

The command reads `http://localhost:5119/openapi/v1.json` by default and fails if any required route or HTTP method disappears. Override the source when necessary:

```bash
OPENAPI_URL=https://localhost:7273/openapi/v1.json npm run test:contract
```

On PowerShell:

```powershell
$env:OPENAPI_URL = 'https://localhost:7273/openapi/v1.json'
npm run test:contract
```

The backend needs a valid `ConnectionStrings__StudentManagementDb` value before it can expose OpenAPI locally.

## Production container

`Dockerfile` performs an Angular production build with Node 24 and serves the output from Nginx. The Nginx configuration provides SPA fallback, immutable caching for hashed assets, security headers and a same-origin `/api` proxy to `student-management-api:8080`.

```bash
docker build -t student-management-web .
docker run --rm -p 8081:8080 student-management-web
```

The public deployment must terminate HTTPS. No runtime configuration file contains secrets.

## Continuous integration

`.github/workflows/frontend-ci.yml` follows the lockfile-based pipeline: install, lint, formatting, type checking and production build. Set the repository variable `OPENAPI_URL` to enable the additional contract job against a deployed or pipeline-hosted API.
