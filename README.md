# Trycore EVM Dashboard

Prueba técnica fullstack para Trycore Colombia.

Esta aplicación permite a líderes de proyecto gestionar proyectos y actividades, registrar avance planificado, avance real y costo real, y calcular automáticamente indicadores de Earned Value Management en tiempo real.

## Funcionalidades principales

- Crear, editar y eliminar proyectos.
- Crear, editar y eliminar actividades de un proyecto.
- Calcular indicadores EVM por actividad.
- Calcular indicadores EVM consolidados por proyecto.
- Visualizar el estado de CPI y SPI.
- Mostrar un dashboard con:
  - Tarjetas resumen del proyecto.
  - Tabla de actividades con indicadores calculados.
  - Badges visuales de estado.
  - Gráfica comparativa de PV, EV y AC por actividad.
- API REST documentada con Swagger/OpenAPI.
- Pruebas unitarias para la lógica de negocio EVM.
- Pruebas de integración para contratos de respuesta de la API.
- Base de datos PostgreSQL usando Docker.

## Fórmulas de Earned Value Management

La aplicación calcula los siguientes indicadores:

| Indicador | Fórmula | Descripción |
|---|---|---|
| BAC | Valor ingresado | Presupuesto total al finalizar |
| PV | % planificado × BAC | Valor planificado |
| EV | % completado × BAC | Valor ganado |
| AC | Valor ingresado | Costo real |
| CV | EV − AC | Variación de costo |
| SV | EV − PV | Variación de cronograma |
| CPI | EV / AC | Índice de desempeño de costo |
| SPI | EV / PV | Índice de desempeño de cronograma |
| EAC | BAC / CPI | Estimación al finalizar |
| VAC | BAC − EAC | Variación al finalizar |

Interpretación:

- CPI > 1: el proyecto está por debajo del presupuesto.
- CPI = 1: el proyecto está dentro del presupuesto.
- CPI < 1: el proyecto está por encima del presupuesto.
- SPI > 1: el proyecto está adelantado.
- SPI = 1: el proyecto está en cronograma.
- SPI < 1: el proyecto está atrasado.

La aplicación maneja casos borde como:

- AC = 0.
- PV = 0.
- Proyectos sin actividades.
- Avance real igual a 0.

## Stack tecnológico

### Backend

- .NET
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- Swagger / OpenAPI
- xUnit
- EF Core InMemory para pruebas de integración

### Frontend

- Angular
- Angular Material
- Chart.js
- TypeScript
- SCSS

### Infraestructura

- Docker
- Docker Compose
- PostgreSQL

## Estructura del repositorio

```text
trycore-evm/
  backend/
    Trycore.Evm.sln
    Trycore.Evm.Api/
    Trycore.Evm.Application/
    Trycore.Evm.Domain/
    Trycore.Evm.Infrastructure/
    Trycore.Evm.Tests.Unit/
    Trycore.Evm.Tests.Integration/

  frontend/
    evm-dashboard/

  docs/
    database-init.sql

  docker-compose.yml
  README.md