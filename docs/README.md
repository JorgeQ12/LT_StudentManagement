# Documentación de Student Management

Este directorio reúne la documentación técnica y operativa del proyecto. La mejor ruta de lectura depende del objetivo:

| Si necesitas… | Empieza por… |
| --- | --- |
| Entender todo el sistema y prepararte para explicarlo | [Guía técnica completa](guia-tecnica-completa.md) |
| Presentar la arquitectura AWS apoyándote en el diagrama | [Guía del diagrama](architecture/student-management-aws-architecture.md) |
| Configurar AWS, Terraform y GitHub Actions | [Terraform para Development](terraform-development.md) |
| Trabajar en la API | [README del backend](../StudentManagementApi/README.md) |
| Trabajar en la interfaz | [README del frontend](../StudentManagementWeb/README.md) |

## Artefactos de arquitectura

- [`student-management-aws-architecture.drawio`](architecture/student-management-aws-architecture.drawio): diagrama editable con tres páginas.
- [`aws-icons/`](architecture/aws-icons/): subconjunto de iconos oficiales de AWS incrustados en el diagrama.

## Orden sugerido de estudio

1. Lee el resumen y los flujos de la [guía técnica completa](guia-tecnica-completa.md).
2. Abre la primera página del `.drawio` e identifica AWS Cloud, región, VPC, zonas y subredes.
3. Recorre la segunda página mientras estudias Terraform y GitHub Actions.
4. Recorre la tercera página para explicar seguridad y el trayecto de una solicitud.
5. Profundiza en los README de backend y frontend según la parte que debas demostrar.

## Fuente de verdad

La documentación explica la implementación, pero el código sigue siendo la fuente de verdad:

- Infraestructura: [`infrastructure/terraform/`](../infrastructure/terraform/)
- Pipeline: [`.github/workflows/development-deploy.yml`](../.github/workflows/development-deploy.yml)
- API: [`StudentManagementApi/`](../StudentManagementApi/)
- Frontend: [`StudentManagementWeb/`](../StudentManagementWeb/)
