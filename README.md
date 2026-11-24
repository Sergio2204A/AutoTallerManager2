# 🔧 AutoTallerManager — API REST para la gestión de un taller automotriz

API desarrollada en **ASP.NET Core (.NET 10)** para administrar la operación diaria de un taller mecánico: clientes, vehículos, órdenes de servicio, repuestos, facturación, pagos y auditoría. Cada acción está protegida con **JWT** y **autorización por roles**.

Proyecto realizado durante mi formación en **Campuslands**.

---

## ✨ Funcionalidades

- **Clientes y vehículos**: registro, consulta paginada con búsqueda, edición y eliminación. Los vehículos se consultan por cliente.
- **Órdenes de servicio**, con un flujo de trabajo completo:
  `crear → asignar mecánico → diagnóstico → aprobación del jefe de taller → aprobación del cliente → iniciar → completar` (también se pueden **rechazar** o **cancelar**).
- **Repuestos e inventario**: CRUD, ajuste de stock y alerta de repuestos con **bajo stock**.
- **Facturación y pagos**: generación de facturas a partir de las órdenes y registro de pagos por factura.
- **Auditoría**: historial de cambios, solo para administradores.
- **Seguridad**: autenticación con JWT, refresh tokens y autorización por roles en cada endpoint.

## 👥 Roles del sistema

| Rol | Puede |
|---|---|
| `Admin` | Todo, incluida la auditoría |
| `Recepcionista` | Crear órdenes, generar facturas, registrar pagos |
| `JefeTaller` | Asignar mecánicos y aprobar órdenes |
| `Mecanico` | Registrar diagnósticos y ajustar stock |
| `JefeAlmacen` / `JefeBodega` | Gestionar repuestos e inventario |
| `Cliente` | Aprobar el trabajo de su vehículo |

## 🏗️ Arquitectura

El proyecto sigue una **arquitectura por capas (Clean Architecture)** con el patrón **CQRS** mediante MediatR:

```
AutoTallerManager/
├── Domain/          → Entidades (Cliente, Vehiculo, OrdenServicio, Factura, Pago, Repuesto, Auditoria…),
│                      value objects (Placa, Cedula) y enums
├── Application/     → Casos de uso (commands y queries) con MediatR y validaciones con FluentValidation
├── Infrastructure/  → DbContext, configuraciones de EF Core, repositorios y Unit of Work
└── Api/             → Controladores, DTOs, configuración de JWT y Swagger
```

## 🛠️ Tecnologías

![.NET](https://img.shields.io/badge/.NET_10-512BD4?style=flat-square&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=flat-square&logo=csharp&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-4169E1?style=flat-square&logo=postgresql&logoColor=white)
![Swagger](https://img.shields.io/badge/Swagger-85EA2D?style=flat-square&logo=swagger&logoColor=black)
![JWT](https://img.shields.io/badge/JWT-000000?style=flat-square&logo=jsonwebtokens&logoColor=white)

- ASP.NET Core Web API
- Entity Framework Core + Npgsql (PostgreSQL)
- MediatR (CQRS) · FluentValidation · Mapster
- ASP.NET Core Identity · JWT Bearer
- Swagger / OpenAPI

## 📡 Endpoints principales

| Recurso | Endpoints |
|---|---|
| `/api/clientes` | `GET` (paginado y con búsqueda) · `GET /{id}` · `POST` · `PUT /{id}` · `DELETE /{id}` |
| `/api/vehiculos` | `GET` · `GET /{id}` · `GET /cliente/{clienteId}` · `POST` · `PUT /{id}` · `DELETE /{id}` |
| `/api/ordenes` | `GET` · `GET /{id}` · `POST` · `POST /{id}/asignar-mecanico` · `/diagnostico` · `/aprobar-jefe` · `/aprobar-cliente` · `/rechazar` · `/iniciar` · `/completar` · `/cancelar` |
| `/api/repuestos` | `GET` · `GET /bajo-stock` · `GET /{id}` · `POST` · `PUT /{id}` · `PATCH /{id}/stock` · `DELETE /{id}` |
| `/api/facturas` | `GET` · `GET /{id}` · `POST` · `GET /{id}/pagos` · `POST /{id}/pagos` |
| `/api/auditorias` | `GET` (solo Admin) |

## 🚀 Cómo ejecutarlo

**Requisitos:** .NET 10 SDK y PostgreSQL.

1. Clona el repositorio:
   ```bash
   git clone https://github.com/Sergio2204A/AutoTallerManager2.git
   cd AutoTallerManager2
   ```
2. Configura tu cadena de conexión y la llave JWT en `Api/appsettings.Development.json` (o con *user secrets*):
   ```json
   "ConnectionStrings": { "Postgres": "Host=localhost;Port=5432;Database=AutoTallerManager;Username=postgres;Password=TU_CLAVE" }
   ```
3. Crea la base de datos con las migraciones (o ejecuta `migration_script.sql`):
   ```bash
   dotnet ef database update --project Infrastructure --startup-project Api
   ```
4. Ejecuta la API:
   ```bash
   dotnet run --project Api
   ```
5. Abre Swagger en `https://localhost:<puerto>/swagger` para probar los endpoints. Usa el botón **Authorize** con tu token JWT.

---

👤 **Autor:** Sergio Andrés Abril Mendoza · [GitHub](https://github.com/Sergio2204A)
