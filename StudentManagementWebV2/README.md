# Student Management Web V2

Frontend nuevo e independiente para la API Lambda de Student Management. Está construido con Angular 22, componentes standalone, Signals, Reactive Forms, SCSS y TypeScript estricto.

## Requisitos

- Node.js 24.19.x
- npm 11.x
- .NET SDK compatible con la solución backend

## Desarrollo local

Primero inicia la Lambda directamente en `https://localhost:7273`:

```powershell
dotnet run --project ..\StudentManagementApi\StudentManagementApi.Presentation.Lambda --urls https://localhost:7273
```

Después inicia este frontend:

```powershell
npm install
npm start
```

La aplicación queda disponible en `https://localhost:4200`. El servidor de Angular redirige `/api` a la Lambda mediante `proxy.conf.json`; HTTPS es necesario para las cookies seguras de autenticación.

## Verificación

```powershell
npm run verify
```

La verificación ejecuta formato, lint, typecheck, pruebas unitarias, build de producción y validación del contrato OpenAPI.

## Contrato y arquitectura

- `docs/openapi.json` contiene el contrato local de 40 operaciones.
- `scripts/verify-openapi.mjs` detecta operaciones o esquemas críticos ausentes.
- El flujo de datos es `Page -> Facade -> Data Access Service -> HttpClient`.
- Autenticación por cookie HttpOnly y protección antiforgery mediante `X-CSRF-TOKEN`.
- La interfaz usa un sistema visual monocromático y un catálogo central de iconos SVG Lucide.
- Selects, calendarios, modales, botones y estados visuales usan componentes propios consistentes, sin widgets nativos del sistema operativo.

Consulta `FRONTEND_CONTEXT.md` para las decisiones arquitectónicas completas.
