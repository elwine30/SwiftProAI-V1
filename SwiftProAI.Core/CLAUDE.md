# SwiftProAI.Core — CLAUDE.md

This is the backend for SwiftProAI, an insurance-tech SaaS platform. It is built on ASP.NET Core 8 using the ABP (ASP.NET Boilerplate) framework with multi-tenancy, layered architecture and SQL Server via Entity Framework Core.

The internal namespace throughout the codebase is `ThinknInsurTech`. All projects, classes and migrations use this prefix.

> **Code wiki:** browsable module-level documentation with dependency graphs is in [`wiki/core/`](../wiki/core/index.md). Open that folder as an Obsidian vault for the graph view.

---

## Build and run

```bash
# Restore dependencies
dotnet restore aspnet-core/ThinknInsurTech.Web.sln

# Run the host (development)
cd aspnet-core/src/ThinknInsurTech.Web.Host
dotnet run

# Apply database migrations
cd aspnet-core/src/ThinknInsurTech.Migrator
dotnet run

# Build solution
dotnet build aspnet-core/ThinknInsurTech.Web.sln

# Run tests
dotnet test aspnet-core/ThinknInsurTech.Web.sln
```

The Angular output is written directly to `ThinknInsurTech.Web.Host/wwwroot`. Run the Angular build first if you need the full UI served from the host.

---

## Project structure

```
aspnet-core/
├── src/
│   ├── ThinknInsurTech.Core                  Domain entities, domain services, ABP module root
│   ├── ThinknInsurTech.Core.Shared           Shared constants, enums, base DTOs
│   ├── ThinknInsurTech.Application           Application services (business use cases)
│   ├── ThinknInsurTech.Application.Shared    Service interfaces and DTOs shared with client
│   ├── ThinknInsurTech.Application.Client    Client-side service wrappers (used by MAUI)
│   ├── ThinknInsurTech.EntityFrameworkCore   DbContext, repositories, EF migrations
│   ├── ThinknInsurTech.GraphQL               GraphQL schema and resolvers
│   ├── ThinknInsurTech.Web.Core              Web infrastructure — JWT, SignalR, Swagger, CORS
│   ├── ThinknInsurTech.Web.Host              Entry point — Startup.cs, Program.cs, appsettings
│   ├── ThinknInsurTech.Web.Public            Public-facing portal (separate entry point)
│   ├── ThinknInsurTech.Migrator              Standalone migration runner
│   └── ThinknInsurTech.Mobile.MAUI           Cross-platform mobile app (iOS, Android, Windows)
├── test/
│   ├── ThinknInsurTech.Tests                 Main test suite
│   ├── ThinknInsurTech.GraphQL.Tests         GraphQL API tests
│   ├── ThinknInsurTech.Test.Base             Shared test infrastructure
│   └── ThinknInsurTech.ConsoleApiClient      Console client for manual API testing
└── docker/                                   Docker and Compose files
```

---

## Architecture

### Layering

The project follows ABP's standard layered pattern:

```
Web.Host / Web.Public   (presentation — controllers, middleware)
        ↓
Web.Core                (web infrastructure — auth, SignalR, Swagger)
        ↓
Application             (use cases — AppServices, DTOs, mapping)
        ↓
Core                    (domain — entities, domain services, events)
        ↓
EntityFrameworkCore     (persistence — DbContext, repos, migrations)
```

Dependencies only flow downward. Cross-cutting concerns (localization, settings, audit) are handled by ABP modules registered at the module level.

### ABP modules

Each project has a module class (e.g. `ThinknInsurTechCoreModule`) that declares its `DependsOn` chain. ABP resolves the full dependency graph at startup. Do not bypass this — register services inside `Initialize()` or `PreInitialize()` on the relevant module class rather than directly in `Startup.cs`.

### Multi-tenancy

The platform is multi-tenant. Every database query is tenant-scoped automatically by ABP's data filter. When writing queries or seeds, always be aware of whether you are in host or tenant context.

---

## Key domain areas

| Folder | Domain |
|--------|--------|
| `Case` | Insurance case lifecycle |
| `Registration` | Claim registration workflow |
| `Companies` | Insurer and company management |
| `Vehicles` | Vehicle records and lookup |
| `Workshops` | Workshop / repairer management |
| `LawFirms` | Legal firm management |
| `Approval` | Multi-step approval workflow |
| `OCR` | OpenAI-backed document OCR |
| `Reports` | Report generation |
| `Audit` | Audit trail and history |
| `Organizations` | Organisation hierarchy |
| `Chat` | Real-time messaging via SignalR |

---

## Database

- Engine: Microsoft SQL Server
- ORM: Entity Framework Core 8
- DbContext: `ThinknInsurTechDbContext`
- Migrations: `aspnet-core/src/ThinknInsurTech.EntityFrameworkCore/Migrations/`
- Seed data: `Migrations/Seed/`

Connection string (development default):
```
Server=localhost; Database=ThinknInsurTechDb1; Trusted_Connection=True; TrustServerCertificate=True;
```

The schema was migrated from PostgreSQL to SQL Server in the initial migration (`20240904011732`). Do not reintroduce PostgreSQL-specific syntax.

To add a migration:
```bash
cd aspnet-core/src/ThinknInsurTech.EntityFrameworkCore
dotnet ef migrations add <MigrationName> --startup-project ../ThinknInsurTech.Web.Host
```

---

## Configuration

| File | Purpose |
|------|---------|
| `appsettings.json` | Base config (development defaults) |
| `appsettings.Production.json` | Production overrides |
| `appsettings.Staging.json` | Staging overrides |
| `appsettings.SIT.json` | System integration testing |
| `appsettings.UAT.json` | User acceptance testing |

Key config sections: `ConnectionStrings`, `Abp.RedisCache`, `App` (CORS, root URLs), `Authentication` (JWT, social login), `Payment` (Stripe, PayPal), `Twilio`, `Recaptcha`, `OCR`, `FileUpload`.

`UseEncryptedConnectionString` can be toggled to encrypt the connection string at rest.

---

## Authentication

- Primary: JWT Bearer (configured in appsettings under `Authentication.JwtBearer`)
- OAuth2/OIDC: OpenIddict (disabled by default, enable via appsettings)
- Social logins: Facebook, Twitter, Google, Microsoft (all disabled by default)
- LDAP: available via `Abp.Zero.Ldap` (disabled by default)
- Multi-factor: supported via ABP Zero

Token security key, issuer and audience are all set in `appsettings.json`. Rotate the security key per environment.

---

## Real-time and background

- **SignalR**: chat and notifications hub in `ThinknInsurTech.Web.Core/Chat/`
- **Hangfire**: background job processing, backed by SQL Server (`Hangfire.SqlServer`)
- **Redis**: distributed cache via `Abp.RedisCache`

---

## Payments

- Stripe: `Stripe.net` — payment processing
- PayPal: `PayPalCheckoutSdk` — checkout flow (sandbox by default)

Both are configured in `appsettings.json` under `Payment`.

---

## OCR

OpenAI-backed OCR is available and enabled by default (`OCR.IsOCREnabled: true`). Max file size is 10 MB. Token costs are configurable in appsettings. OCR services live under `ThinknInsurTech.Application/OCR/`.

---

## File upload

Allowed types: `jpeg`, `jpg`, `png`, `pdf`. Max size: 10 MB. Root storage path is configurable via `Folder.root` in appsettings.

---

## API surface

- REST: standard ABP dynamic API controllers (auto-generated from AppServices)
- GraphQL: endpoint at `/graphql`, playground at `/graphql/playground`
- Swagger: at `/swagger` (development only by default)

---

## Docker

Compose files are under `aspnet-core/docker/`. The web host exposes ports 80 and 443. Infrastructure services (SQL Server, Redis) are defined in `docker-compose.infrastructure.yml`. Run the migrator container before the host on first deploy.

---

## Testing conventions

- Unit and integration tests live under `test/ThinknInsurTech.Tests/`
- Base classes and fixtures are in `test/ThinknInsurTech.Test.Base/`
- GraphQL tests are separate under `test/ThinknInsurTech.GraphQL.Tests/`
- Use `ThinknInsurTech.ConsoleApiClient` for manual exploratory API testing against a running instance

---

## Code generation

`AspNetZeroRadTool/` contains the ABP RAD (rapid application development) tool with templates for generating CRUD modules. Templates cover Angular components, MVC views and server-side service/entity/migration scaffolding. Do not hand-edit generated template files.

---

## Common conventions

- All AppService methods must correspond to a declared interface in `Application.Shared`
- DTOs live alongside their AppService in `Application` or in `Application.Shared` if shared with the client
- Use `CustomDtoMapper.cs` (AutoMapper profile) for all entity-to-DTO mappings — do not map inline
- Repository pattern: use `IRepository<TEntity, TPrimaryKey>` from ABP — do not add raw DbContext queries to AppServices
- Audit logging is enabled for anonymous users — do not disable unless explicitly required
- Version: all projects share `common.props` at version 13.1.0
