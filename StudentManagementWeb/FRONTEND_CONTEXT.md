# Frontend Context

## Proyecto

Portal académico para registro estudiantil y administración de programas, cursos, profesores y estudiantes. La interfaz está dirigida a usuarios de Colombia y usa español; el código utiliza inglés.

## Stack

- Angular 22, TypeScript estricto y componentes standalone
- Signals para estado y RxJS para HTTP
- Reactive Forms
- SCSS, CSS Custom Properties y diseño minimalista monocromático
- HttpClient y Angular CDK
- Iconos SVG Lucide a través de un componente central

## Arquitectura

Feature First. El flujo de negocio es `Page -> Facade -> Data Access Service -> HttpClient -> Lambda API`. Las pages orquestan; los componentes de feature son presentacionales; los facades exponen Signals readonly.

## Estructura

- `core/`: configuración, autenticación, guards, interceptors y errores HTTP
- `layout/`: shell autenticado
- `shared/`: componentes y servicios transversales reales
- `features/`: authentication, administration, academic-programs, courses, professors, students, enrollment y student-profile
- `styles/`: tokens, base, abstracts y utilities

## Infraestructura compartida

- Modal global accesible para información, éxito, advertencia, error y confirmación
- Loader HTTP global con reference counting y exclusión mediante `HttpContextToken`
- Normalización central de RFC Problem Details
- Sesión en memoria y autenticación por cookie segura
- Design tokens y componentes UI compartidos
- Catálogo tipado de iconos SVG para navegación, acciones y estados
- Select y datepicker propios como `ControlValueAccessor`, sin desplegables ni calendarios nativos

## Decisiones arquitectónicas

- `docs/openapi.json` es la fuente local del contrato HTTP.
- La API es una AWS Lambda ASP.NET Core expuesta bajo `/api`.
- Autenticación mediante JWT en cookie HttpOnly `__Host-student_access_token`; no se almacenan tokens en Web Storage.
- Toda request usa credenciales. Requests inseguras usan `X-CSRF-TOKEN`, obtenido desde `GET /api/Authentication/GenerateAntiforgeryToken`.
- Roles públicos del contrato: `Administrator` y `Student`.
- Problem Details incluye `code` y `traceId`; errores de validación incluyen `errors` por campo.
- La paginación usa `pageNumber`, `pageSize`, `totalCount` e `items`.
- No se usa NgRx.

## Convenciones

- Rutas de feature lazy-loaded.
- Servicios HTTP viven en `data-access/` y no contienen lógica visual.
- Mensajes importantes y confirmaciones usan el modal global; no se usan toasts o snackbars.
- Fechas `DateOnly` viajan como `YYYY-MM-DD`; fechas de auditoría son ISO 8601 con zona.
- Los controles visuales dependientes del sistema operativo se reemplazan por componentes propios accesibles por teclado.

## Estado actual

Proyecto Angular 22 independiente creado desde cero. Contrato OpenAPI de 40 operaciones y 38 esquemas guardado localmente. Infraestructura transversal, autenticación, administración, matrícula y perfil estudiantil implementados contra la Lambda.

## Tarea actual

Validar y mantener la aplicación completa contra la Lambda.

## Pendientes relacionados

- Añadir pruebas end-to-end cuando exista un entorno estable con datos semilla.
