# Guía del diagrama de arquitectura AWS

El archivo [`student-management-aws-architecture.drawio`](student-management-aws-architecture.drawio) contiene tres páginas editables y utiliza iconos oficiales de AWS incrustados en el archivo.

## Orden de presentación

1. **01 - Arquitectura AWS:** límites físicos y lógicos del despliegue.
2. **02 - CI-CD y Terraform:** creación, actualización y publicación del ambiente.
3. **03 - Flujo y seguridad:** recorrido de una solicitud y controles por capa.

## Convenciones visuales

| Contenedor | Significado |
| --- | --- |
| `AWS Cloud` | Todo lo que pertenece a la cuenta y servicios AWS del sistema. |
| `Región AWS · us-east-1` | Recursos regionales. CloudFront queda fuera porque opera sobre la red global de borde. |
| `VPC · 10.42.0.0/16` | Recursos o interfaces que utilizan direccionamiento y controles de la red privada. |
| `Zona de disponibilidad` | Separación física seleccionada por Terraform mediante las dos primeras AZ disponibles. |
| `Subred privada` | Segmento sin ruta pública. Terraform crea dos redes `/20`. |

Los colores siguen las categorías de AWS: naranja para cómputo, morado para networking, verde para almacenamiento, magenta para observabilidad, rojo para seguridad y violeta para bases de datos.

## Página 1: arquitectura AWS

### Entrada global

El navegador solo conoce el dominio HTTPS de CloudFront. CloudFront es global y queda fuera del contenedor regional.

CloudFront tiene dos orígenes privados/lógicos:

- **S3:** comportamiento predeterminado para `index.html`, JavaScript, CSS y assets.
- **API Gateway:** comportamientos ordenados para `/api/*`, `/swagger*` y `/openapi/*`.

S3 no es público. La bucket policy permite `s3:GetObject` únicamente al servicio CloudFront y solo desde el ARN de la distribución mediante Origin Access Control.

### Servicios regionales fuera de la VPC

S3, API Gateway, Secrets Manager y CloudWatch son servicios administrados de AWS. No se despliegan dentro de las subredes del proyecto.

- API Gateway es `REGIONAL` y utiliza integración `AWS_PROXY`.
- Secrets Manager guarda conexión, clave JWT y credenciales iniciales.
- CloudWatch recibe logs de ejecución de Lambda.

### VPC y subredes

Terraform crea:

- VPC `10.42.0.0/16`.
- Subred A `10.42.0.0/20`.
- Subred B `10.42.16.0/20`.
- Cada subred vive en una zona de disponibilidad diferente.

No se crean subredes públicas, Internet Gateway ni NAT Gateway. Por eso la Lambda no puede salir libremente a Internet.

### Lambda

La función se ejecuta con .NET 10, 1024 MB y timeout de 120 segundos. Su `vpc_config` referencia ambas subredes privadas y el Security Group de Lambda.

La Lambda necesita dos caminos:

- TCP 1433 hacia RDS.
- HTTPS 443 hacia el endpoint de Secrets Manager.

Aunque el icono de Lambda aparece una sola vez, la etiqueta indica que AWS puede crear conectividad mediante ENIs en las dos subredes configuradas.

### VPC Interface Endpoint

El endpoint es de tipo `Interface`, habilita Private DNS y corresponde a `com.amazonaws.us-east-1.secretsmanager`.

Cuando el SDK llama al hostname regional de Secrets Manager, DNS lo resuelve hacia la IP privada del endpoint. El tráfico permanece dentro de AWS y no necesita NAT.

En la implementación actual el endpoint se crea únicamente en la primera subred. Funciona para toda la VPC, pero una versión productiva debería considerar un endpoint por zona para mejorar disponibilidad y evitar dependencia entre AZ.

### RDS

RDS ejecuta SQL Server Express en `db.t3.medium`, con 20 GB gp3, una sola AZ y sin acceso público. El DB subnet group incluye las dos subredes, aunque una instancia Single-AZ se ejecuta en una sola zona a la vez.

El Security Group de la base únicamente acepta TCP 1433 desde el Security Group de Lambda.

## Página 2: CI/CD y Terraform

La página separa tres conceptos:

### Repositorio y detección

Un push a `development` inicia GitHub Actions. El job obtiene el historial completo y compara `github.event.before` con `GITHUB_SHA`.

Genera dos outputs:

- `backend=true|false`.
- `frontend=true|false`.

Cambios de infraestructura o del workflow activan ambos. Una ejecución manual también realiza redeploy completo.

### Bootstrap

El contenedor AWS Cloud de bootstrap representa recursos creados una sola vez:

- Bucket de estado remoto.
- Proveedor OIDC de GitHub.
- Rol IAM de despliegue.

El token OIDC se intercambia por credenciales temporales. GitHub no almacena Access Keys permanentes.

### Estado y despliegue selectivo

`terraform init` siempre se ejecuta para abrir el estado remoto y adquirir el lock.

- Si cambió backend o infraestructura, se ejecuta `terraform apply`.
- Si cambió solo frontend, no se modifica la infraestructura; se leen los outputs existentes.
- Después de backend, el workflow invoca la API hasta recibir HTTP 200.
- Después de frontend, sincroniza S3 e invalida CloudFront.

## Página 3: flujo y seguridad

Esta página sigue una solicitud:

1. El navegador establece HTTPS con CloudFront.
2. CloudFront decide el origen por path.
3. Los assets se recuperan desde S3 mediante OAC.
4. Las rutas de API llegan a API Gateway.
5. API Gateway entrega el evento proxy a Lambda.
6. Lambda aplica autenticación, roles, antiforgery, validación y casos de uso.
7. Lambda consulta Secrets Manager y RDS por los caminos autorizados.
8. Lambda envía diagnóstico a CloudWatch Logs.
9. La respuesta regresa por API Gateway y CloudFront.

El contenedor regional muestra que API Gateway, Secrets Manager y CloudWatch están fuera de la VPC. El contenedor VPC incluye únicamente la conectividad privada de Lambda y RDS.

## Defensa por capas

| Capa | Control |
| --- | --- |
| Borde | CloudFront fuerza HTTPS y es el único dominio visible. |
| Contenido | S3 es privado y solo permite acceso desde la distribución. |
| Red | RDS no tiene IP pública; Security Groups permiten únicamente los puertos necesarios. |
| Secretos | Secrets Manager, IAM mínimo y PrivateLink. |
| Aplicación | JWT, estado de cuenta, autorización por roles, antiforgery y rate limiting. |
| Datos | Invariantes de dominio, transacciones serializables, constraints y concurrencia optimista. |
| Operación | CloudWatch Logs y `traceId` en Problem Details. |

## Guion corto para explicar el diagrama

> El usuario entra por CloudFront, que es nuestro único punto público. CloudFront sirve Angular desde un S3 privado o envía las rutas de API a API Gateway. API Gateway usa integración proxy con una Lambda .NET 10. La función está asociada a dos subredes privadas para comunicarse con RDS SQL Server, que no es público. Como la VPC no tiene NAT, la Lambda obtiene sus secretos mediante un VPC Interface Endpoint. Terraform crea todos estos recursos y GitHub Actions despliega backend y frontend de forma selectiva usando credenciales temporales OIDC.

## Preguntas comunes

### ¿Por qué API Gateway dice `authorization = NONE`?

Porque API Gateway no valida el JWT. La autenticación, las cookies, los roles y antiforgery se implementan en ASP.NET Core. `NONE` no significa que los endpoints de negocio sean públicos.

### ¿Por qué CloudFront también está frente a la API?

Permite un solo dominio para Angular y la API, simplifica cookies estrictas y evita CORS en el despliegue AWS.

### ¿Por qué no hay NAT Gateway?

La función solo necesita RDS y Secrets Manager. RDS está dentro de la VPC y Secrets Manager se alcanza por PrivateLink, por lo que un NAT agregaría costo y una salida más amplia que no se necesita.

### ¿Por qué RDS aparece en una sola zona si hay dos subredes?

El DB subnet group debe ofrecer subredes en varias zonas. La instancia configurada es Single-AZ y utiliza una de ellas; Multi-AZ sería una decisión adicional de producción.

### ¿Qué ocurre en el primer arranque?

En Development la Lambda obtiene el secreto, aplica migraciones pendientes si están habilitadas y garantiza el administrador inicial de forma idempotente.

## Fuentes de los iconos

Los SVG provienen del paquete oficial [AWS Architecture Icons](https://aws.amazon.com/architecture/icons/) y están incrustados en el `.drawio`. Los contenedores AWS Cloud, región, VPC y subredes utilizan la librería de formas AWS de diagrams.net.

Consulta también la [guía técnica completa](../guia-tecnica-completa.md) y la [guía operativa de Terraform](../terraform-development.md).

