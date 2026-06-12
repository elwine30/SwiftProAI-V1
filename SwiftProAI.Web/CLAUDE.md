# SwiftProAI.Web — CLAUDE.md

This is the Angular 17 frontend for SwiftProAI, an insurance-tech SaaS platform. It communicates with the `SwiftProAI.Core` ASP.NET Core backend via auto-generated NSwag service proxies and uses ABP framework conventions for authentication, multi-tenancy and localisation.

The build output is written directly into `SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Host/wwwroot`.

---

## Build and run

All commands run from `angular/`.

```bash
# Install dependencies
npm install

# Start development server (http://localhost:4200)
npm start

# Start with Hot Module Replacement
npm run hmr

# Production build (outputs to wwwroot)
npm run publish

# Kubernetes build
npm run publish-k8s

# Regenerate API service proxies from backend OpenAPI
npm run nswag

# Run unit tests
npm test

# Run E2E tests (Playwright)
cd ../ui-tests-playwright && npx playwright test

# Lint
npm run lint
```

The backend must be running before you regenerate proxies (`npm run nswag`) or run E2E tests.

---

## Project structure

```
angular/
├── src/
│   ├── main.ts                       Bootstrap entry point
│   ├── root.module.ts                Root module — APP_INITIALIZER, session init
│   ├── root-routing.module.ts        Root routes — lazy loads account and app modules
│   ├── account/                      Unauthenticated routes (login, register, payment)
│   │   ├── login/
│   │   ├── register/
│   │   ├── password/
│   │   ├── payment/                  Stripe and PayPal checkout
│   │   └── shared/
│   ├── app/
│   │   ├── app.module.ts             Main layout module — 13 themes, chat, notifications
│   │   ├── app-routing.module.ts     App routes — gated by AppRouteGuard
│   │   ├── main/                     User-facing features (dashboard, cases, reports)
│   │   ├── admin/                    Admin features (users, roles, tenants, settings)
│   │   └── shared/                   Layout, services, guards, common components
│   ├── shared/
│   │   ├── common/                   Session, auth, UI customisation services
│   │   ├── service-proxies/          Auto-generated NSwag API clients
│   │   ├── utils/
│   │   ├── helpers/
│   │   ├── AppConsts.ts              Global constants
│   │   └── AppEnums.ts               Shared enums
│   ├── assets/                       Static assets, Metronic theme files, images
│   └── environments/                 Environment config per build target
├── nswag/                            NSwag config for proxy generation
├── Dockerfile                        nginx production image
├── angular.json                      Build config — 4 environments, output to wwwroot
├── package.json
└── tsconfig.json
```

---

## Module hierarchy

```
RootModule (root.module.ts)
└── AppModule (app.module.ts)        — layout, themes, chat
    ├── AccountModule (lazy)         — login, register, password, payment
    └── (app routes)
        ├── MainModule (lazy)        — user dashboard, cases, invoices, registration
        └── AdminModule (lazy)       — users, roles, tenants, settings, audit logs
```

All feature modules under `app/main/` and `app/admin/` are lazy loaded with `preload: true`.

---

## Key files

| File | Purpose |
|------|---------|
| `src/main.ts` | Bootstrap; enables production mode; HMR support |
| `src/root.module.ts` | APP_INITIALIZER — runs AppPreBootstrap before app loads |
| `src/root-routing.module.ts` | Root routes; manages body CSS classes per route |
| `src/app/app-routing.module.ts` | App routes; applies AppRouteGuard; shows spinner |
| `src/app/shared/common/auth/auth-route-guard.ts` | Permission and auth guard |
| `src/app/shared/common/auth/app-auth.service.ts` | Logout and token cleanup |
| `src/shared/common/session/app-session.service.ts` | User/tenant session state |
| `src/shared/service-proxies/zero-template-http-configuration.service.ts` | HTTP error handling and 401 recovery |
| `src/shared/AppConsts.ts` | Global constants (URLs, cookie names) |
| `angular.json` | Build config — output path, assets, environment targets |
| `nswag/service.config.nswag` | NSwag codegen config |

---

## Authentication and routing

### Guards

- **AppRouteGuard** (`CanActivate`, `CanActivateChild`, `CanLoad`): checks `AppSessionService` for a valid user, then validates permissions via `PermissionCheckerService`. Redirects to login if unauthenticated. Handles refresh token retry. Routes users to the correct dashboard based on role.
- **AccountRouteGuard**: applied to `/account/**` — redirects authenticated users away from login/register.
- **DirtyFormGuard**: prevents navigation away from forms with unsaved changes.

### Token management

Tokens are stored via ABP's built-in encrypted storage. The `ZeroTemplateHttpConfigurationService` intercepts 401 responses and attempts token refresh before logging out. Do not add custom HTTP interceptors without checking what ABP already handles.

---

## API integration

Service proxies under `src/shared/service-proxies/` are auto-generated by NSwag from the backend OpenAPI spec. Never hand-edit these files — regenerate them with `npm run nswag` after backend changes.

`API_BASE_URL` is injected at runtime via a factory in `root.module.ts` that reads from `appconfig.json` (environment-specific file loaded at boot).

---

## Themes

The app supports 13 switchable Metronic themes (`default`, `theme2` through `theme13`). Each theme has its own brand and layout component pair declared in `app.module.ts`. The active theme CSS class is applied dynamically to the document body by `AppUiCustomizationService`. Dark mode is a toggle component separate from the theme selection.

Do not hard-code theme-specific CSS classes in components — use the customisation service or theme-aware utility classes.

---

## State management

There is no NgRx or similar store. State flows through:

- **AppSessionService** — authenticated user and tenant
- **abp.session / abp.auth / abp.settings** — ABP global JS objects (set at boot)
- **AppUiCustomizationService** — theme and UI state
- **LocalForage** — persistent local storage (initialised in root module)

---

## Real-time features

SignalR is used for chat, push notifications and live updates. `ChatSignalrService` manages the WebSocket connection. The SignalR client is loaded as a global script (see `angular.json` scripts section). Do not import `@microsoft/signalr` directly in components — use the existing service.

---

## Build environments

| Config | Output | Notes |
|--------|--------|-------|
| `development` | local dev server | source maps on, vendor chunk |
| `hmr` | local dev server | Hot Module Replacement |
| `sit` | wwwroot | optimised, minified |
| `production` | wwwroot | fully optimised |
| `k8s` | wwwroot | production + k8s appconfig |

Environment files in `src/environments/` select the correct `appconfig.*.json` at build time. App config (API base URL, tenant info) is loaded at runtime from `appconfig.json` — not baked into the bundle.

---

## Testing

### Unit tests (Karma + Jasmine)

```bash
npm test
```

Output: `coverage/` directory. Runs in headless Chrome.

### E2E tests (Playwright)

```bash
cd ui-tests-playwright
npx playwright test
```

Config: `playwright.config.ts`. Screenshots are compared with a pixel tolerance of 100. Traces are recorded on first retry. The backend must be running before E2E tests execute.

---

## Linting and formatting

```bash
npm run lint        # ESLint
```

Prettier is configured but not wired to a lint script — run it manually or via your editor. ESLint config is in `.eslintrc.js`.

---

## Common conventions

- Service proxies are generated — never edit files under `src/shared/service-proxies/` by hand
- Permissions are checked via `PermissionCheckerService` — do not add ad-hoc `if (user.isAdmin)` checks
- All API calls go through the generated proxy services — do not use `HttpClient` directly in components
- Lazy load every new feature module; add `preload: true` to the route data
- Place shared components used by both `main` and `admin` in `app/shared/`; utilities used app-wide go in `src/shared/`
- Reactive Forms are preferred over Template Forms for complex forms
- Use `AppConsts` for magic strings and URLs — do not inline them in components
- The `abp.*` global namespace is available at runtime (set by ABP JS scripts loaded in `index.html`) — use it only where the ABP module does not already expose a typed Angular service
